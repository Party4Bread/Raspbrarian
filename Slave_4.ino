#include <TinyWireS.h>
#define Slave 4

const int led = 3; //LED 데이터 핀
#define led_on()   digitalWrite(led, HIGH)
#define led_off()  digitalWrite(led, LOW)

void receiveEvent(uint8_t howMany);
volatile uint8_t data;

void setup() {
 TinyWireS.begin(Slave);
 TinyWireS.onReceive(receiveEvent);
 pinMode(led, OUTPUT);
}

void loop() {
    TinyWireS_stop_check();
}
void receiveEvent(uint8_t howMany){
  if( howMany <1){
    return;
  }
  if ( howMany > 16){
    return;
  }
  howMany--;

  while(1< TinyWireS.available()){
    char data = TinyWireS.receive();
  
  if(data == 1){
      led_on();
  }
  if(data == 2){
    led_off();
  }
  }
}


