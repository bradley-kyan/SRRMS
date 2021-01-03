#include <SPI.h>
#include <MFRC522.h>
#include <Wire.h>
#include <LiquidCrystal.h>

#define RST_PIN         9          // Configurable, see typical pin layout above
#define SS_PIN          8         // Configurable, see typical pin layout above

int errorDelay = 1000;
int successDelay = 200;

String lastUid;

MFRC522 mfrc522(SS_PIN, RST_PIN);  // Create MFRC522 instance
LiquidCrystal lcd( 8, 9, 4, 5, 6, 7 );

void setup() {

Serial.begin(9600);   // Initialize serial communications with the PC

	SPI.begin();    // Init SPI bus
	SPI.setClockDivider(SPI_CLOCK_DIV16);

  pinMode(10, OUTPUT);
  digitalWrite(10, HIGH);
  
	mfrc522.PCD_Init();		// Init MFRC522
  Serial.println("Successfully initalized");
	delay(200);				// Optional delay. Some board do need more time after init to be ready, see Readme
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
      tone(2, 2600, errorDelay); //error tone to D5
      delay(errorDelay);
  }
  void success(){
      tone(2, 3300, successDelay); //Success tone to D5
      delay(successDelay);
      tone(2, 3400, successDelay);
  }
};

void loop() {

  tones Tones;
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
      Tones.success();
       lcd.begin(16, 2);
       lcd.print(uid);
    }
    delay(1000);
  }
}
