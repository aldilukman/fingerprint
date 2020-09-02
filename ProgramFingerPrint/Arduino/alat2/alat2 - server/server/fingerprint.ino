void setupFingerPrint()
{

  // set the data rate for the sensor serial port
  finger.begin(57600);

  if (finger.verifyPassword()) {
    Serial.println("P1_Found Sensor_");
  } else {
    Serial.println("P1_Not Found Sensor_");
    while (1) {
      delay(5000);
      SendData("Alarm Sensor finger pada gate tidak ditemukan harus di restart");
    }
  }
}


uint8_t getFingerprintID() {
  uint8_t p = finger.getImage();
  switch (p) {
    case FINGERPRINT_OK:
      Serial.println("P1_Checking Data_");
      break;
    default:
      //Serial.println("P_Failed Read_");
      return p;
  }

  // OK success!

  p = finger.image2Tz();
  switch (p) {
    case FINGERPRINT_OK:
      Serial.println("P_Image converted_");
      break;

    default:
      Serial.println("P_Failed Read_");
      return p;
  }

  // OK converted!
  p = finger.fingerFastSearch();
  if (p == FINGERPRINT_OK) {
    Serial.println("P_Found a print match_");
    Serial.println("B3_");
  } else if (p == FINGERPRINT_NOTFOUND) {
    Serial.println("P_FF Not Found_");
    SendData("N_NotFound_");
    Serial.println("B3_");
    delay(1000);
    Serial.println("B3_");
    delay(1000);
    Serial.println("B3_");
    delay(1000);
    return p;
  } else {
    Serial.println("P_Failed");
    return p;
  }

  // found a match!
  Serial.print("P_Found ID #");
  Serial.print(finger.fingerID);
  Serial.println("_");
  SendData(String(finger.fingerID));
  Serial.println("B3_");
  downloadFingerprintTemplate(finger.fingerID);
  return finger.fingerID;
}

void deleteFingerprint(int id) {
  uint8_t p = -1;
  Serial.println("P1_Delete User_");
  Serial.print("P_#");
  Serial.println(id);
  p = finger.deleteModel(id);

  if (p == FINGERPRINT_OK) {
    Serial.print("P_Delete ");
    Serial.print(id);
    Serial.println("_");
    SendData("Sukses");
  } else {
    Serial.println("P_Failed Delete_");
  }
}

uint8_t downloadFingerprintTemplate(uint16_t id)
{
  Serial.print("Attempting to load #"); Serial.println(id);
  int p = finger.loadModel(id);
  switch (p) {
    case FINGERPRINT_OK:
      Serial.print("Template "); Serial.print(id); Serial.println(" loaded");
      break;
    case FINGERPRINT_PACKETRECIEVEERR:
      Serial.println("Communication error");
      return p;
    default:
      Serial.print("Unknown error "); Serial.println(p);
      return p;
  }

  Serial.print("Attempting to get #"); Serial.println(id);
  p = finger.getModel();       // FP_UPLOAD = UPCHAR 0x08  -getModel() for Char Buffer 1 and getM odel2() for Char Buffer 2-
  switch (p) {
    case FINGERPRINT_OK:
      Serial.print("Template "); Serial.print(id); Serial.println(" transferring:");
      break;
   default:
      Serial.print("Unknown error "); Serial.println(p);
      return p;
  }

  uint8_t bytesReceived[900];

  int i = 0;
  while (i <= 900 ) {
      if (Serial3.available()) {
          bytesReceived[i++] = Serial3.read();
      }
  }
  Serial.println("Decoding packet...");

  // Filtering The Packet
  int a = 0, x = 3;
  Serial.print("uint8_t packet2[] = {");
  for (int i = 10; i <= 832; ++i) {
      a++;
      if (a >= 129)
        {
          i+=10;
          a=0;
          Serial.println("};");Serial.print("uint8_t packet");Serial.print(x);Serial.print("[] = {");
      x++;
        }
      else
      {
         Serial.print("0x"); printHex(bytesReceived[i-1] , 2); Serial.print(", ");//Serial.print("/"); 
      }
  }
  Serial.println("};");
  Serial.println("COMPLETED\n");
}

void emptyData() {
  finger.emptyDatabase();
  Serial.println("P1_Deleted All Data_");
  SendData("Sukses");
}


void SendData(String numberFF) {
  EthernetClient client3;
  if (client3.connect(IPAddress(192, 168, 1, 1), 12346))
  {
    Serial.print("Kirim Data ");
    Serial.println(numberFF);
    client3.println(numberFF);
    client3.stop();
  }
  else
  {
    Serial.println("P1_Server Not Connect");
    Serial.println("P_Server Not Connect");
  }
}

void printHex(int num, int precision) {
    char tmp[16];
    char format[128];
 
    sprintf(format, "%%.%dX", precision);
 
    sprintf(tmp, format, num);
    Serial.print(tmp);
}
