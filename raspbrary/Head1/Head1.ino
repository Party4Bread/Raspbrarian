#include <SerialCommand.h>
#define LED_LOW      22
#define LED_HIGH     

SerialCommand     serialCommand;
void setup()
{
  serialCommand.addCommand("ledon", SetLedOn );
  serialCommand.addCommand("ledoff", SetLedOff );
  for ( byte i = LED_LOW; i <= LED_HIGH; i++ ) {
    pinMode( i, OUTPUT );
  }
}
void loop()
{
  
}

