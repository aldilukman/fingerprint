#include <Adafruit_Fingerprint.h>

#include <SPI.h>
#include <Ethernet.h>


const int buttonPin = 5;     // the number of the pushbutton pin
const int ledPin =  13;
int buttonState = 0;
int NoRegistrasi = 0;
EthernetServer server(12345);
String dataReceive ;
String dataCode ;
String dataValue ;
unsigned long interval = 30000; // the time we need to wait
unsigned long intervalLCD = 1000; // the time we need to wait
unsigned long previousMillis = 0; // millis() returns an unsigned long.

Adafruit_Fingerprint finger = Adafruit_Fingerprint(&Serial3);
void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  byte mac[] = {0xFE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
  IPAddress ip(192, 168, 1, 7);
  Ethernet.begin(mac, ip);
  server.begin();
  setupFingerPrint();
  pinMode(buttonPin, INPUT);

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
      dataCode = dataReceive.substring(0, dataReceive.indexOf('_'));
      dataValue = dataReceive.substring(dataReceive.indexOf('_') + 1);
      Serial.println(dataReceive);

      if (dataCode == "R") {
        //waiting data enroll
        NoRegistrasi = dataValue.toInt();
        Serial.println("P1_Register_");
        Serial.println(NoRegistrasi);
        unsigned long currentMillis = millis(); // grab current time
        unsigned long currentMillisLCD = millis(); // grab current time
        int detikLCD =1;
        Serial.println("P_1 dari 30_");
        uint8_t bytesReceived[900];
        client.stop();
        EthernetClient client2 = server.available();

        while (!client2) {
          
          previousMillis = millis();
          if ((unsigned long)(previousMillis-currentMillis) >= interval) {
            break;
          }
          if((unsigned long)(previousMillis-currentMillisLCD) >= intervalLCD) {
            detikLCD++;
            Serial.print("P_");
            Serial.print(detikLCD);
            Serial.println(" dari 30_");
            currentMillisLCD = millis();
          }
          client2 = server.available();
          //getFingerprintID();
          int buttonState = digitalRead(buttonPin);
          if (buttonState == HIGH) {
            SendData("Open");
            digitalWrite(ledPin, HIGH);
            Serial.println("P1_Gate Di Buka_");
            Serial.println("O_Open_");
            Serial.println("M_Open_");
            //delay(1000);
          } else {
            //digitalWrite(ledPin, LOW);
            //Serial.println("P1_Gate Di lose_");
          }
        }
        if (client2) {
          int i = 0;
          while (i <= 900 ) {
            if (client2.available()) {
              bytesReceived[i++] = client2.read();
            }
          }
          client2.stop();

          uint8_t packet2[128], packet3[128], packet4[128], packet5[128], packet6[128], packet7[128];

          int a = 0, x = 3;
          for (int i = 10; i <= 832; ++i) {
            a++;
            if (a >= 129)
            {
              i += 10;
              a = 0;
              x++;
            }
            else
            {
              if (x == 3) {
                packet2[a - 1] = bytesReceived[i - 1];
              } else if (x == 4) {
                packet3[a - 1] = bytesReceived[i - 1];
              } else if (x == 5) {
                packet4[a - 1] = bytesReceived[i - 1];
              } else if (x == 6) {
                packet5[a - 1] = bytesReceived[i - 1];
              } else if (x == 7) {
                packet6[a - 1] = bytesReceived[i - 1];
              } else if (x == 8) {
                packet7[a - 1] = bytesReceived[i - 1];
              }
            }
          }
          int p = finger.uploadModelDY50(packet2, packet3, packet4, packet5, packet6, packet7);    // Simpan di Char Buffer 01
          switch (p) {
            case FINGERPRINT_OK:
              Serial.println("P1_SUKSES");
              //Serial.read();
              break;
            case FINGERPRINT_PACKETRECIEVEERR:
              Serial.println("P1_Communication error");
              return p;
            case FINGERPRINT_BADPACKET:
              Serial.println("P1_Bad packet");
              return p;
            default:
              {
                //Serial.println("\n==>[SUKSES] UploadModel = ");
                //return p;
              }
          }
          p = finger.storeModel(NoRegistrasi);                  // taruh di ID = 0 pada flash memory FP
          if (p == FINGERPRINT_OK) {
            Serial.println("P1_Stored ");
            SendData("Sukses");
            Serial.println("P1_Register Selesai_");
          } else if (p == FINGERPRINT_PACKETRECIEVEERR) {
            Serial.println("P1_Communication error");
            //return p;
          } else if (p == FINGERPRINT_BADLOCATION) {
            Serial.println("P1_Could not store in that location");
            //return p;
          } else if (p == FINGERPRINT_FLASHERR) {
            Serial.println("P1_Error writing to flash");
            //return p;
          } else {
            Serial.println("P1_Unknown error");
            //return p;
          }
        }
        else {
          Serial.println("P1_Registrasi Gagal_");
        }


      } else if (dataCode == "I") {
        setupFingerPrint();
      }
      else if (dataCode == "d") {
        dataValue = dataValue.substring(0, dataValue.indexOf('_'));
        deleteFingerprint(dataValue.toInt());
      }
      else if (dataCode == "D") {
        emptyData();
      }

    }
    client.stop();
  }
  getFingerprintID();
  int buttonState = digitalRead(buttonPin);
  if (buttonState == HIGH) {
    SendData("Open");
    digitalWrite(ledPin, HIGH);
    Serial.println("P1_Gate Di Buka_");
    Serial.println("O_Open_");
    Serial.println("M_Open_");
    delay(1000);
  } else {
    //digitalWrite(ledPin, LOW);
    //Serial.println("P1_Gate Di lose_");
  }
}
