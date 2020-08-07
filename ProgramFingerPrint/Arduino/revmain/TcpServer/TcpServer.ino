

#define MACADDRESS 0x00,0x01,0x02,0x03,0x04,0x05
#define MYIPADDR 192,168,1,6

#include <UIPEthernet.h>
#include <LiquidCrystal_PCF8574.h> //mengincludekan library LCD
#include <Wire.h>
#include <SoftwareSerial.h>

SoftwareSerial mySerial(3, 4);
LiquidCrystal_PCF8574 lcd(0x27);
EthernetServer server = EthernetServer(12345);


void setup() {
  uint8_t mac[6] = {MACADDRESS};
  uint8_t myIP[4] = {MYIPADDR};
  Ethernet.begin(mac, myIP);
  server.begin();

  mySerial.begin(9600);

  Wire.begin();
  Wire.beginTransmission(0x27);
  Wire.endTransmission();

  lcd.begin(16, 2); //prosedur pemanggilan LCD
  lcd.setBacklight(255);


}
void loop() {

  size_t size;

  if (EthernetClient client = server.available())
  {
    while ((size = client.available()) > 0)
    {
      uint8_t* msg = (uint8_t*)malloc(size + 1);
      memset(msg, 0, size + 1);
      size = client.read(msg, size);
      String dataReceive = (char*)msg;
      String dataCode = dataReceive.substring(0,dataReceive.indexOf('_'));
      String dataValue = dataReceive.substring(dataReceive.indexOf('_')+1);
      
      if (dataCode == "O") {
        setLCD1(dataValue);
        setLCD2("Open Gate");
        digitalWrite(2, HIGH);
        digitalWrite(6, HIGH);
        digitalWrite(A2, HIGH);
        delay(3000);
        digitalWrite(2, LOW);
        digitalWrite(6, LOW);
        digitalWrite(A2, LOW);
        setLCD2("Close Gate");
      }
      mySerial.println(dataReceive);
      //setLCD2(dataReceive);
      free(msg);
    }
    client.stop();
  }
  if (mySerial.available()) {
    String dataSerial = mySerial.readStringUntil('\n');
    String dataCode = dataSerial.substring(0,dataSerial.indexOf('_'));
    String dataValue = dataSerial.substring(dataSerial.indexOf('_')+1);
      
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
    else{
      sendData(dataValue);
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

void sendData(String numberFF) {
  EthernetClient client;
  if (client.connect(IPAddress(192, 168, 1, 1), 12346))
  {
    client.println(numberFF);
    client.stop();
  }
  else
  {
    setLCD1("Server Not Connect");
    setLCD2("Server Not Connect");
  }
}
