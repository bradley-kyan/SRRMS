#include <SPI.h>
#include <MFRC522.h>

#define RST_PIN         9          // Configurable, see typical pin layout above
#define SS_PIN          8         // Configurable, see typical pin layout above

String lastUid;
MFRC522 mfrc522(SS_PIN, RST_PIN);  // Create MFRC522 instance

void setup() {

Serial.begin(9600);   // Initialize serial communications with the PC
	while (!Serial);		// Do nothing if no serial port is opened (added for Arduinos based on ATMEGA32U4)
	
	SPI.begin();    // Init SPI bus
	SPI.setClockDivider(SPI_CLOCK_DIV8);

  pinMode(10, OUTPUT);
  digitalWrite(10, HIGH);
      
	mfrc522.PCD_Init();		// Init MFRC522
  Serial.println("Successfully initalized");
	delay(4);				// Optional delay. Some board do need more time after init to be ready, see Readme
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

void loop() {
	// Reset the loop if no new card present on the sensor/reader. This saves the entire process when idle.
	if ( ! mfrc522.PICC_IsNewCardPresent()) {
		return;
	}
  else{
    
    String uid = getID();
    if(lastUid == uid){ //check if current card has the same uid as last scanned card
      Serial.println("Card Already Scanned");
    }
    else if(uid == "" || uid == " "){ //Check if UID is empty
      return;
    }
    else{
      Serial.print("Card detected, UID: "); Serial.println(uid);
      lastUid = uid; //save current card to local memory
    }
  }
  delay(2000);
}
