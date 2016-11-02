#define LED_LOW 22
#define LED_HIGH 53
String preon = "0";
void setup()
{
  Serial.begin(9600);
  for ( byte i = LED_LOW; i <= LED_HIGH; i++ ) {
    pinMode( i, OUTPUT );
  }
}
void loop()
{
  if(Serial.available())
  {
    String received = Serial.readString();
    Serial.write(received.c_str());
    digitalWrite(atoi(preon.c_str()), LOW);  
    digitalWrite(atoi(received.c_str()), HIGH);  
    preon=received;
  }
}
