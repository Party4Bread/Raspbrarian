#include <SerialCommand.h>
#include<Wire.h>
#define Slave 0
SerialCommand seri;
void setup() {
  Wire.begin();
  Serial.begin(9600);
  //seri.addCommand("ledon", SetLedOn );
  //seri.addCommand("ledoff", SetLedOff );
}

void loop() {
   while(true){
    if(Serial.available()>1){
      String MON=Serial.readString() = Serial.read();   
      if(MON == "MON"){
        Wire.beginTransmission(Slave); 
        Wire.write(0); 
        Wire.endTransmission();
        while(true)
        {
          if(Wire.available()>1){
            String NEW=Serial.readString() = Wire.read();
            if(NEW == "NEW"){
              Serial.print(NEW);
              break;
            }
            else
            {
              return;
            }
          }
        }
        while(true){
          if(Serial.available()>1)
          {
            String C = Serial.readString() = Serial.read();
            Wire.beginTransmission(Slave);
            Wire.write(C.c_str());            
            Wire.endTransmission();
            break;
          }            
        }       
        while(true){
          if(Wire.available()>1)
          {
            String OKorFAIL =Serial.readString() = Wire.read();
            Serial.print(OKorFAIL);
          }
        }
      }
    }
  }   

}
//void SetLedOn(){}

//void SetLedOff(){}

