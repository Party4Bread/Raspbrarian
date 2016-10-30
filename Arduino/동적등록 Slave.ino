#include<TinyWireS.h>
#define Slave 0

void requestEvent(void);
void receiveEvent(uint8_t howmany);

void setup() {
    TinyWireS.begin(Slave);
    TinyWireS.onRequest(requestEvent);
    TinyWireS.onReceive(receiveEvent);
}

void loop() {

}
void receiveEvent(uint8_t howmany){
  if( howmany <1){
    return;
  }
  if( howmany >16){
    return;
  }
  char data =  TinyWireS.receive();
  howmany--;

  TinyWireS.begin(data);
  TinyWireS.send('OK');
}

void requestEvent(void){
  TinyWireS.send('New');
  
}

