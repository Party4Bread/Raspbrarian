// 문자 '1' ~ '6' Slave 주소
// 문자 '1'은  LED를 켜는 명령어
// 문자 '2'는 LED를 끄는 명령어 
// 예를 들면 Slave 1번만 LED를 켜는 경우 Serial로 아래 명령어를 보낸다.
//
// 11  여서기 1은(0x31 즉 십진수 49) 이다.
//
// 예를 들어 Slave 3를 끄는  경우 아래 명령어를 보내면된다.
//  32 여기서 32는 (0x33,0x32 십진수 51,50)이다.
//

#include 

int SLAVE[6] = {1, 2, 3, 4, 5, 6};

void setup(){
		Wire.begin();
		Serial.begin(9600); 
}

void loop() {
		if (Serial.available()) {
				char e = Serial.read();
				byte c = e - 48; // '1' => 49 , '2' => 50 ,, '6' => 54                          
				byte onoff = Serial.read(); //  다음에 '1' 혹은 '2'가 온다고 가정하는 경우
				if (c < 7) {
						Wire.beginTransmission(SLAVE[c-1]}; // SLAVE[0] => 1, SLAVE[1] = > 2 ,, SLAVE[5] => 6 그래서 [c-1]로 해야 정확하게 원하는 숫자가 나옴. 
						if(onoff=='1') // 문자 1이 오면 켜는 명령을 던진다.
								Wire.write(1); // LED를 켜는  명령
						if(onoff=='2') // 문자 2가 오면 켜는 명령을 던진다.
								Wire.write(2); // LED를  끄는  명령
								
						Wire.endTransmission(SLAVE[c-1]};

						delay(10);

						// i2c_communication(c-1); // 사용핮지 않아 코멘트 처리함.
				}
		}
}

// 아래는 아마도 사용하지 않을 확율 높음.
void i2c_communication(int i) {
		// 12 바이트 크기의 데이터 요청
		Wire.requestFrom(SLAVE[i], 12); 
		// 12 바이트 모두 출력할 때까지 반복
		for (int j = 0 ; j < 12 ; j++) { 
				// 수신 데이터 읽기
				char c = Wire.read(); 
				// 수신 데이터 출력
				Serial.print(c); 
		}
		Serial.println();
}
