
#include <LiquidCrystal_PCF8574.h> //mengincludekan library LCD
#include <Wire.h>

LiquidCrystal_PCF8574 lcd(0x27);
String dataReceive ;
String dataCode ;
String dataValue ;

void setup() {
  Serial.begin(115200);

  Wire.begin();
  Wire.beginTransmission(0x27);
  Wire.endTransmission();
  pinMode(6, OUTPUT);
  lcd.begin(16, 2); //prosedur pemanggilan LCD
  lcd.setBacklight(255);


}
void loop() {


  size_t size;

  
  if (Serial.available()) {
    dataReceive = Serial.readStringUntil('\n');
    dataCode = dataReceive.substring(0,dataReceive.indexOf('_'));
    dataValue = dataReceive.substring(dataReceive.indexOf('_')+1);
      
    if (dataCode == "P") {
      dataValue = dataValue.substring(0,dataValue.indexOf('_'));
      setLCD2(dataValue);
    }else if (dataCode == "P1") {
      dataValue = dataValue.substring(0,dataValue.indexOf('_'));
      setLCD1(dataValue);
    }
    else if (dataCode == "B3"){
        digitalWrite(2, HIGH);
        delay(500);
        digitalWrite(2, LOW);
    }
    else if (dataCode == "O"){
        digitalWrite(2, HIGH);
        digitalWrite(6, HIGH);
        digitalWrite(A2, HIGH);
        delay(3000);
        digitalWrite(2, LOW);
        digitalWrite(6, LOW);
        digitalWrite(A2, LOW);
    }
    
  }
  
}

void setLCD1(String dataDisplay){
  lcd.clear();
  lcd.setCursor(00,00); //set pada baris 1 dan kolom 1
  lcd.print("                ");
  lcd.setCursor(00,00); //set pada baris 1 dan kolom 1
  lcd.print(dataDisplay);
}
void setLCD2(String dataDisplay2){
  lcd.setCursor(00,1); //set pada baris 2 dan kolom 1
  lcd.print("                ");
  lcd.setCursor(00,1); //set pada baris 2 dan kolom 1
  lcd.print(dataDisplay2);
}
