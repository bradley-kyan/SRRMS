#include <SPI.h>
#include <MFRC522.h>
#include <Wire.h>
#include <Ethernet.h>
#include <EthernetUdp.h>
#include <TimeLib.h>

#define RST_PIN         7          // Configurable, see typical pin layout above
#define SS_PIN          6         // Configurable, see typical pin layout above

String DeviceID = "i19ahbaj16";
int errorDelay = 1000;
int successDelay = 200;

// Enter a MAC address and IP address for your controller below.
// The IP address will be dependent on your local network:
byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };

EthernetClient client;
String lastUid;

EthernetUDP Udp;
unsigned int localPort = 8888;
IPAddress timeServer(129,6,15,28);

String currentTime;
const int timeZone = 12;

MFRC522 mfrc522(SS_PIN, RST_PIN);  // Create MFRC522 instance

void setup() {

Serial.begin(9600);   // Initialize serial communications with the PC

  SPI.begin();    // Init SPI bus
  SPI.setClockDivider(SPI_CLOCK_DIV16);
  
  mfrc522.PCD_Init();   // Init MFRC522
  Serial.println("Initalized MFRC522");
  Ethernet.begin(mac);
    Serial.println("Connection Successful");
  delay(200);
  Serial.print("Updating Time.");
  delay(500);

  Serial.println();
  Udp.begin(localPort);
  setSyncProvider(getNtpTime);
}

  String getID(){
    if ( ! mfrc522.PICC_ReadCardSerial()) { //Since a PICC placed get Serial and continue
      return "";
    }
    String uidString =  String(mfrc522.uid.uidByte[0]);
    uidString +=  String(mfrc522.uid.uidByte[1]);
    uidString +=  String(mfrc522.uid.uidByte[2]);
    uidString +=  String(mfrc522.uid.uidByte[3]);
    mfrc522.PICC_HaltA(); // Stop reading
    return uidString;
  }
  

class tones{
  public:
  void error(){
      tone(5, 2600, errorDelay); //error tone to D5
      delay(errorDelay);
  }
  void success(){
      tone(5, 3300, successDelay); //Success tone to D5
      delay(successDelay);
      tone(5, 3400, successDelay);
  }
};

/*-------- NTP code ----------*/

const int NTP_PACKET_SIZE = 48; // NTP time is in the first 48 bytes of message
byte packetBuffer[NTP_PACKET_SIZE]; //buffer to hold incoming & outgoing packets

time_t getNtpTime()
{
  while (Udp.parsePacket() > 0) ; // discard any previously received packets
  Serial.println("Transmit NTP Request");
  sendNTPpacket(timeServer);
  uint32_t beginWait = millis();
  while (millis() - beginWait < 1500) {
    int size = Udp.parsePacket();
    if (size >= NTP_PACKET_SIZE) {
      Serial.println("Receive NTP Response");
      Udp.read(packetBuffer, NTP_PACKET_SIZE);  // read packet into the buffer
      unsigned long secsSince1900;
      // convert four bytes starting at location 40 to a long integer
      secsSince1900 =  (unsigned long)packetBuffer[40] << 24;
      secsSince1900 |= (unsigned long)packetBuffer[41] << 16;
      secsSince1900 |= (unsigned long)packetBuffer[42] << 8;
      secsSince1900 |= (unsigned long)packetBuffer[43];
      return secsSince1900 - 2208988800UL + timeZone * SECS_PER_HOUR;
    }
  }
  Serial.println("No NTP Response :-(");
  return 0; // return 0 if unable to get the time
}

// send an NTP request to the time server at the given address
void sendNTPpacket(IPAddress &address)
{
  // set all bytes in the buffer to 0
  memset(packetBuffer, 0, NTP_PACKET_SIZE);
  // Initialize values needed to form NTP request
  // (see URL above for details on the packets)
  packetBuffer[0] = 0b11100011;   // LI, Version, Mode
  packetBuffer[1] = 0;     // Stratum, or type of clock
  packetBuffer[2] = 6;     // Polling Interval
  packetBuffer[3] = 0xEC;  // Peer Clock Precision
  // 8 bytes of zero for Root Delay & Root Dispersion
  packetBuffer[12]  = 49;
  packetBuffer[13]  = 0x4E;
  packetBuffer[14]  = 49;
  packetBuffer[15]  = 52;
  // all NTP fields have been given values, now
  // you can send a packet requesting a timestamp:                 
  Udp.beginPacket(address, 123); //NTP requests are to port 123
  Udp.write(packetBuffer, NTP_PACKET_SIZE);
  Udp.endPacket();
}

void loop() {
  tones Tones;
  Ethernet.maintain();
  currentTime = (String)year() + "-" + (String)month() + "-" + (String)day()+ " " +(String)hour() + ":" + (String)minute() + ":" + (String)second(); //Update current time to be sent
  // Reset the loop if no new card present on the sensor/reader. This saves the entire process when idle.
  if ( ! mfrc522.PICC_IsNewCardPresent()) {
    return;
  }
  else{
    String uid = getID();
    if(lastUid == uid){ //check if current card has the same uid as last scanned card
      Serial.println("Card Already Scanned");
      Tones.error();
    }
    else if(uid == "" || uid == " "){ //Check if UID is empty
      Tones.error();
    }
    else{
      Serial.print("Card detected, UID: "); Serial.println(uid);
      lastUid = uid; //save current card to local memory
      int i = 0;
      retry:
      if(client.connect(IPAddress(192,168,1,7), 29882)){
        Serial.println("Client connected");
        String data = DeviceID+"?"+uid+"?"+currentTime+"<EOF>";
        client.print(data);
        Serial.println(data+"\n");
          String rData;
          while(client.available()){
              rData += (char)client.read();
              }
          if (!client.available()) {
            Serial.println(rData + "\n");
             Tones.success();
              client.stop();
              return; 
          }
        }
      else{
        if(i == 0){
          goto retry;
          i++;
        }
        else{
          Tones.error();
          }
        }  
    }
  }
}
