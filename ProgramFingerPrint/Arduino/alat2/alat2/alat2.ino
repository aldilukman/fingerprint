#include <Adafruit_Fingerprint.h>
const int buttonPin = 5;     // the number of the pushbutton pin
const int ledPin =  13; 
int buttonState = 0;  
SoftwareSerial mySerial(4, 3);
Adafruit_Fingerprint finger = Adafruit_Fingerprint(&mySerial);
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  setupFingerPrint();
  
}

void loop() {
  // put your main code here, to run repeatedly:
  getFingerprintID();
  if(Serial.available() > 0) {
    String dataReceive = Serial.readStringUntil('\n');
    String dataCode = dataReceive.substring(0,dataReceive.indexOf('_'));
    String dataValue = dataReceive.substring(dataReceive.indexOf('_')+1);
    if (dataCode == "R") {
      enroll(dataValue.toInt());
    }else if(dataCode == "I"){
      setupFingerPrint();
    }
    else if (dataCode == "d") {
      dataValue = dataValue.substring(0,dataValue.indexOf('_'));
      deleteFingerprint(dataValue.toInt());
    }
    else if (dataCode == "D") {
      emptyData();
    }
  }
  

  if (!digitalRead(5)) {
    digitalWrite(ledPin, HIGH);
    Serial.println("P1_Gate Di Buka_");
    Serial.println("O_Open_");
    Serial.println("M_Open_");
    delay(1000);
  } else {
    digitalWrite(ledPin, LOW);
    
  }
}
