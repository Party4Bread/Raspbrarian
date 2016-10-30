#include<Wire.h>
#define Slave 0

void setup() {
  Wire.begin();
  Serial.begin(9600);

}

void loop() {
   Wire.beginTransmission(Slave);
   Wire.requestFrom(Slave, 1);
   while(Wire.available()>1){
   char a = Wire.read() ;
   Serial.print(a);
   }
   Wire.endTransmission();
   
   Wire.beginTransmission(Slave);
   while(Serial.available()>1){
   char e = Serial.read();
   byte c = e- 48;
   Wire.write(c);
   }
   Wire.endTransmission();

   Wire.beginTransmission(Slave);
    while(Wire.available()>1){
      char b = Wire.read();
      Serial.print(b);
    }
   }

