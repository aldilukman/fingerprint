#include <Adafruit_Fingerprint.h>

//#include <SPI.h>
#include <UIPEthernet.h>


const int buttonPin = 5;     // the number of the pushbutton pin
const int ledPin =  13; 
int buttonState = 0;  
EthernetServer server(12345);
String dataReceive ;
String dataCode ;
String dataValue ;

Adafruit_Fingerprint finger = Adafruit_Fingerprint(&Serial3);
void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  byte mac[] = {0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED};
  IPAddress ip(192,168,1,6);
  Ethernet.begin(mac, ip);
  server.begin();
  setupFingerPrint();
  
}

void loop() {
  // put your main code here, to run repeatedly:
  
  size_t size;
 EthernetClient client = server.available();
  if (client)
  {
    while ((size = client.available()) > 0)
    {
      uint8_t* msg = (uint8_t*)malloc(size + 1);
      memset(msg, 0, size + 1);
      size = client.read(msg, size);
      dataReceive = (char*)msg;
      free(msg);
      dataCode = dataReceive.substring(0,dataReceive.indexOf('_'));
      dataValue = dataReceive.substring(dataReceive.indexOf('_')+1);
      
      //if (dataCode == "O") {
        
      //}
      Serial.println(dataReceive);
      if (dataCode == "R") {
        enroll(dataValue.toInt());
      }
    }
    client.stop();
  }
  
}
