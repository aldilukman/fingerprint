void setupFingerPrint()  
{

  // set the data rate for the sensor serial port
  finger.begin(57600);
  
  if (finger.verifyPassword()) {
     Serial.println("P1_Found Sensor_");
  } else {
     Serial.println("P1_Not Found Sensor_");
    while (1) { delay(1); }
  }
}
void enroll(int idNumber)                     // run over and over again
{
  while (!  getFingerprintEnroll(idNumber) );
}

uint8_t getFingerprintEnroll(int id) {
  
  int p = -1;
   Serial.println("P1_Register_");
   Serial.println("P_Insert Finger_");
  while (p != FINGERPRINT_OK) {
    p = finger.getImage();
    switch (p) {
    case FINGERPRINT_OK:
       Serial.println("P_Image taken_");
      break;
    case FINGERPRINT_NOFINGER:
       //Serial.println("P_No Finger_");
      break;
    default:
       Serial.println("P_Failed Read_");
      break;
    }
  }

  // OK success!

  p = finger.image2Tz(1);
  switch (p) {
    case FINGERPRINT_OK:
       Serial.println("P_Image converted_");
      break;
    
    default:
       Serial.println("P_Failed Read_");
      return p;
  }
  
   Serial.println("P_Remove finger_");
  delay(2000);
  p = 0;
  while (p != FINGERPRINT_NOFINGER) {
    p = finger.getImage();
  }
  p = -1;
  Serial.println("P_Insert Finger_");
  while (p != FINGERPRINT_OK) {
    p = finger.getImage();
    switch (p) {
    case FINGERPRINT_OK:
       Serial.println("P_Image taken_");
      break;
    case FINGERPRINT_NOFINGER:
       //Serial.println("P_Place finger_");
      break;
    
    default:
       Serial.println("P_Failed read_");
      break;
    }
  }

  // OK success!

  p = finger.image2Tz(2);
  switch (p) {
    case FINGERPRINT_OK:
       Serial.println("P_Image converted_");
      break;
    default:
       Serial.println("P_Failed Read_");
      return p;
  }
  
  // OK converted!
   Serial.println("P_Creating_"); 
  
  p = finger.createModel();
  if (p == FINGERPRINT_OK) {
     Serial.println("P_Prints matched_");
  } 
  else {
     Serial.println("P_Prints not matched_");
    return p;
  }   
  
  p = finger.storeModel(id);
  if (p == FINGERPRINT_OK) {
     Serial.println("P_Stored_");
     Serial.println("S_Sukses");
  } 
  else {
     Serial.println("P_Error writing_");
    return p;
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
  p = finger.fingerSearch();
  if (p == FINGERPRINT_OK) {
     Serial.println("P_Found a print match_");
     Serial.println("B3_");
  } else if (p == FINGERPRINT_NOTFOUND) {
     Serial.println("P_FF Not Found_");
     Serial.println("N_NotFound_");
     Serial.println("B3_");
     delay(1000);
     Serial.println("B3_");
     delay(1000);
     Serial.println("B3_");
     delay(1000);
    return p;
  } else {
     //Serial.println("P_Failed");
    return p;
  }   
  
  // found a match!
   Serial.print("P_Found ID #");
   Serial.print(finger.fingerID);
   Serial.println("_");
   Serial.print("M_");
   Serial.println(finger.fingerID);
   Serial.println("B3_");
  return finger.fingerID;
}

void deleteFingerprint(uint8_t id) {
  uint8_t p = -1;
   Serial.println("P1_Delete User_");
   Serial.print("P_#");
   Serial.println(id);
  p = finger.deleteModel(id);

  if (p == FINGERPRINT_OK) {
     Serial.print("P_Delete ");
     Serial.print(id);
     Serial.println("_");
     Serial.println("S_Sukses");
  } else {
     Serial.println("P_Failed Delete_");
  } 
}

void emptyData(){
  finger.emptyDatabase();
   Serial.println("P1_Deleted All Data_"); 
   Serial.println("S_Sukses");
}
