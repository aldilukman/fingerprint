void setupFingerPrint()
{

  // set the data rate for the sensor serial port
  finger.begin(57600);

  if (finger.verifyPassword()) {
    Serial.println("P1_Found Sensor_");
  } else {
    Serial.println("P1_Not Found Sensor_");
    while (5000) {
      SendData("Alarm Sensor finger pada gate tidak ditemukan harus di restart");
    }
  }
}
void enroll(int idNumber)                     // run over and over again
{
  while (!  getFingerprintEnroll(idNumber) );
}

uint8_t getFingerprintEnroll(int id) {

  int p = -1;
  //SendData("Print_Register_");
  SendData("Print_Insert Finger");
  while (p != FINGERPRINT_OK) {
    p = finger.getImage();
    switch (p) {
      case FINGERPRINT_OK:
        //SendData("Print_Image taken");
        break;
      case FINGERPRINT_NOFINGER:
        //Serial.println("P_No Finger");
        break;
      default:
        SendData("Print_Failed Read");
        break;
    }
  }

  // OK success!

  p = finger.image2Tz(1);
  switch (p) {
    case FINGERPRINT_OK:
      //SendData("Print_Image converted");
      break;

    default:
      SendData("Print_Failed Read");
      return p;
  }

  SendData("Print_Remove finger");
  delay(2000);
  p = 0;
  while (p != FINGERPRINT_NOFINGER) {
    p = finger.getImage();
  }
  p = -1;
  SendData("Print_Insert Finger");
  while (p != FINGERPRINT_OK) {
    p = finger.getImage();
    switch (p) {
      case FINGERPRINT_OK:
        //SendData("Print_Image taken");
        break;
      case FINGERPRINT_NOFINGER:
        //Serial.println("Print_Place finger");
        break;

      default:
        SendData("Print_Failed read");
        break;
    }
  }

  // OK success!

  p = finger.image2Tz(2);
  switch (p) {
    case FINGERPRINT_OK:
      //SendData("Print_Image converted");
      break;
    default:
      SendData("Print_Failed Read");
      return p;
  }

  // OK converted!
  //SendData("Print_Creating");

  p = finger.createModel();
  if (p == FINGERPRINT_OK) {
    //SendData("Print_Prints matched");
  }
  else {
    SendData("Print_Prints not matched");
    return p;
  }

  p = finger.storeModel(id);
  if (p == FINGERPRINT_OK) {
    SendData("Print_Stored");
    downloadFingerprintTemplate(id);
  }
  else {
    SendData("Print_Error writing");
    return p;
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
  i = 0;
  EthernetClient client;
  if (client.connect(IPAddress(192, 168, 1, 7), 12345))
  {
    while (i <= 900 ) {
      client.write(bytesReceived[i++]);
    }
    client.stop();
  }else{
    SendData("Print_Not Connect to Gate");
  }
  
  Serial.println("Decoding packet...");

  // Filtering The Packet
  int a = 0, x = 3;
  Serial.print("uint8_t packet2[] = {");
  for (int i = 10; i <= 832; ++i) {
    a++;
    if (a >= 129)
    {
      i += 10;
      a = 0;
      Serial.println("};"); Serial.print("uint8_t packet"); Serial.print(x); Serial.print("[] = {");
      x++;
    }
    else
    {
      Serial.print("0x"); printHex(bytesReceived[i - 1] , 2); Serial.print(", "); //Serial.print("/");
    }
  }
  Serial.println("};");
  SendData("Print_COMPLETED");
  
}



void printHex(int num, int precision) {
  char tmp[16];
  char format[128];

  sprintf(format, "%%.%dX", precision);

  sprintf(tmp, format, num);
  Serial.print(tmp);
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
