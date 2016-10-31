#include<TinyWireS.h>
#define Slave 0
#define led_on()   digitalWrite(led, HIGH)
#define led_off()  digitalWrite(led, LOW)
const int led = 3; 
void requestEvent(void);
void receiveEvent(uint8_t howmany);

void setup() {
    TinyWireS.begin(Slave);
    TinyWireS.onRequest(requestEvent);
    TinyWireS.onReceive(receiveEvent);
    pinMode(led, OUTPUT);
}
void loop() {
  
}


void receiveEvent(uint8_t howmany){
  if( howmany <1){
    return;
  }
  if(   howmany >16){
    return;
  }
  while(true){
    if(TinyWireS.available()>1){
    String M = TinyWireS.receive();
    if(M == 0){
      TinyWireS.send('NEW');
    }
    delay(10);
  int TIME=0;
  while(TIME < 400){
    String NewAdress = TinyWireS.receive();
    if(TinyWireS.available()>1){
      TinyWireS.begin(NewAdress.toInt());
      TinyWireS.send('OK');
    }
  TIME++; 
  delay(10);
  }
  if(!TIME<400){
    TinyWireS.send('Fail');
  }
  /*while(true){
    if(TinyWireS.available()>1){
    String z = TinyWireS.receive();
    }if(z == 1){
      led_on();
    }
    else{
      return;
    }*/
   }
  }
}
void requestEvent(void){
  TinyWireS.send('OK'); 
}

