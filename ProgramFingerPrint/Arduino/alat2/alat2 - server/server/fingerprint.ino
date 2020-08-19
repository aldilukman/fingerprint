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
