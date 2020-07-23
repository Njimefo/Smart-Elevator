#pragma region Etagen



  #define Etage2 3
#define Etage1 16
#define Etage0 24
int ActualEtage=0 ;

  #pragma endregion



 
const int button1Pin = 46;
const int button2Pin = 47;
const int button3Pin = 45;
int buttonStand1 = 0;
int buttonStand2 = 0;
int buttonStand3 = 0;
#pragma region LCD Elemente
/*
  The circuit:
   LCD RS pin to digital pin 12
   LCD Enable pin to digital pin 11
   LCD D4 pin to digital pin 5
   LCD D5 pin to digital pin 4
   LCD D6 pin to digital pin 3
   LCD D7 pin to digital pin 2
   LCD R/W pin to ground
   LCD VSS pin to ground
   LCD VCC pin to 5V
   10K resistor:
   ends to +5V and ground
*/

// include the library code:
#include <LiquidCrystal.h>
int LCD_Digital_RS_Pin = 49;
int LCD_Digital_Enable_Pin = 48;
int LCD_Display_D4_Pin = 51;
int LCD_Display_D5_Pin = 50;
int LCD_Display_D6_Pin = 53;
int LCD_Display_D7_Pin = 52;
byte A_slash[8] = { // Array of bytes
  B00001,  // B stands for binary formatter and the 5 numbers are the pixels
  B00010,
  B00100,
  B01000,
  B10000,
  B00000,
  B00000,
  B00000,
};

byte B_slash[8] = { // Array of bytes
  B00000,  // B stands for binary formatter and the 5 numbers are the pixels
  B00000,
  B00000,
  B10000,
  B01000,
  B00100,
  B00010,
  B00001,
};


byte C_slash[8] = { // Array of bytes
  B00000,  // B stands for binary formatter and the 5 numbers are the pixels
  B00000,
  B00000,
  B00001,
  B00010,
  B00100,
  B01000,
  B10000,
};
byte D_slash[8] = { // Array of bytes
  B10000,  // B stands for binary formatter and the 5 numbers are the pixels
  B01000,
  B00100,
  B00010,
  B00001,
  B00000,
  B00000,
  B00000,
};
byte Barre[8] = { // Array of bytes
  B00100,  // B stands for binary formatter and the 5 numbers are the pixels
  B00100,
  B00100,
  B00100,
  B00100,
  B00100,
  B00100,
};
// initialize the library with the numbers of the interface pins
LiquidCrystal lcd(LCD_Digital_RS_Pin, LCD_Digital_Enable_Pin, LCD_Display_D4_Pin, LCD_Display_D5_Pin, LCD_Display_D6_Pin, LCD_Display_D7_Pin);
int A = 4;
int B = 5;
int C = 6;
int D = 7;
int barre = 8;
#pragma endregion

#pragma region StepperMotor(SM)
#include <Stepper.h>

#include <AFMotor.h>
// Connect a stepper motor with 48 steps per revolution (7.5 degree)
// to motor port #2 (M3 and M4)
AF_Stepper motor(48, 1);


#pragma endregion

//*************************************************************************************************************
#pragma region Ultraschall Sensor

//Triger Pin des Ultraschall Sensors
int US_TrigerPin = 38 ;

//Echo Pin des Ultraschall Sensors
int US_EchoPin = 39;

//Ergebnis nach Berechnung der tatsÃ¤chlichen LÃ¤nge
long US_Ergebnis = 0;

//Gemessener Wert
long US_Messung = 0;


#pragma endregion
//*************************************************************************************************************


//-------------------------------------------------------------
void setup()
{
  //Set speed
  motor.setSpeed(100);  // 10 rpm

 

  // set up the LCD's number of columns and rows:
 lcd.begin(18, 2);
  SetUP_Pfeile();
  LCD_StandarText();
  US_Setup();
  Serial.begin(9600);



}
//--------------------------------------------------------


//----------------------------------------------------
void loop()
{
   LCD_ScrollText();

 US_Messen();

 buttonStand3 = digitalRead(button3Pin);
 buttonStand1 = digitalRead(button1Pin);
    buttonStand2 = digitalRead(button2Pin);

    //Buttn 1 pressed
if(buttonStand1&&!buttonStand2&&!buttonStand3&&ActualEtage!=0)
{


  FahreZuEtage(0);
}
else if(!buttonStand1&&buttonStand2&&!buttonStand3&&ActualEtage!=1)
{


  FahreZuEtage(1);
}
else if(!buttonStand1&&!buttonStand2&&buttonStand3&&ActualEtage!=2)
{


  FahreZuEtage(2);
}
else return;


  US_Messen();
  Serial.print("Messung : ");
  Serial.println(US_Messung);
  Serial.print("Ergebnis : ");
  Serial.println(US_Ergebnis);
  //delay(1000);


  TestFahrtRichtung();
  Fahstuhl_Fahrt(true, 13);
}
//------------------------------------------------------

//------------------------------------------------------
void SetUP_Pfeile()
{
  lcd.createChar(A, A_slash);
  lcd.createChar(B, B_slash);
  lcd.createChar(C, C_slash);
  lcd.createChar(D, D_slash);
  lcd.createChar(barre, Barre);
}
//---------------------------------------------------


//-------------------------------------------------
void Fahstuhl_Fahrt(bool hoch, int cursorX)
{
  if (hoch)
  {
    lcd.setCursor(0, 0);
    lcd.write("Fahrtrichtung");
    lcd.setCursor(0, 1);
    lcd.write("Aufwaerts");
    lcd.setCursor(cursorX, 0);
    lcd.write(A);
    lcd.setCursor(cursorX + 1, 0);
    lcd.write(D);
    lcd.setCursor(cursorX, 1);
    lcd.write(barre);
    lcd.setCursor(cursorX + 1, 1);
    lcd.write(barre);


  }
  else
  {
    lcd.setCursor(0, 0);
    lcd.write("Fahrtrichtung");
    lcd.setCursor(0, 1);
    lcd.write("Abwaerts");
    lcd.setCursor(cursorX, 1);
    lcd.write(B);
    lcd.setCursor(cursorX + 1, 1);
    lcd.write(C);
    lcd.setCursor(cursorX, 0);
    lcd.write(barre);
    lcd.setCursor(cursorX + 1, 0);
    lcd.write(barre);
  }
}
//-------------------------------------------------------


///<summary>
/// Set up the ultra schall sensor pins
///</summary>
void US_Setup()
{
  pinMode(US_TrigerPin, OUTPUT);
  pinMode(US_EchoPin, INPUT);
}

//-------------------------------------------------------


void US_Messen()
{
  //Triger(Messinstrument) ausschalten
  digitalWrite(US_TrigerPin, LOW);
  //2 Millisekunden abwarten
  delayMicroseconds(2);
  //Triger(Messinstrument) einschalten
  digitalWrite(US_TrigerPin, HIGH);
  //10 Millisekunden abwarten
  delayMicroseconds(10);
  //Triger(Messinstrument) ausschalten
  digitalWrite(US_TrigerPin, LOW);
  //gemessenen Wert holen
  US_Messung = pulseIn(US_EchoPin, HIGH);
  US_Ergebnis = (US_Messung / 2) / 29;

}
//-----------------------------------------------------
void TestFahrtRichtung()
{

  //    bool richtungHoch= true;
  //    for(int i=0;i<=15;i++) {
  //Fahstuhl_Fahrt(richtungHoch,i);
  //        delay(1000); // 1 second delay
  //        lcd.clear(); // Write a character to the LCD
  //        richtungHoch = !richtungHoch;
  //      }

  Serial.print(US_Ergebnis);
  
  if ( US_Ergebnis >= 16)
  {
    motor.step(100, BACKWARD, DOUBLE);
  }

  else
  {
      motor.step(0, FORWARD, DOUBLE);
  }

}
//---------------------------------------------------------------
void LCD_StandarText()
{
  lcd.setCursor(0, 0);
  lcd.print("Fahrstuhl ");
  lcd.setCursor(0, 1);
  lcd.print("Eingeschaltet");
}
//---------------------------------------------------------------

void FahreZuEtage(int etage)
{

     lcd.clear();

  
      int differenz = abs(ActualEtage - etage);
      bool hoch;
      if ((ActualEtage - etage) > 0) hoch = false;
      else hoch = true;

      if(etage == 1 && hoch)
      {
              Fahstuhl_Fahrt(hoch, 13);
        while(US_Ergebnis >= Etage1)
        {
          motor.step(100, FORWARD, DOUBLE);
          
          US_Messen();
        }
            motor.step(0, FORWARD, DOUBLE);
      }
      else if(etage == 1 && !hoch)
      {
         Fahstuhl_Fahrt(hoch, 13);
         while(US_Ergebnis <= Etage1)
        {
          motor.step(100, BACKWARD, DOUBLE);
            US_Messen();
        }
            motor.step(0, FORWARD, DOUBLE);
      }
       else if(etage == 2 && hoch)
      {
         Fahstuhl_Fahrt(hoch, 13);
        while(US_Ergebnis >= Etage2)
        {
          motor.step(100, FORWARD, DOUBLE);
            US_Messen();
        }
            motor.step(0, FORWARD, DOUBLE);
      }
      else if(etage == 2 && !hoch)
      {
         Fahstuhl_Fahrt(hoch, 13);
         while(US_Ergebnis <= Etage2)
        {
          motor.step(100, BACKWARD, DOUBLE);
            US_Messen();
        }
            motor.step(0, FORWARD, DOUBLE);
      }
             else if(etage == 0 && hoch)
      {
         Fahstuhl_Fahrt(hoch, 13);
        while(US_Ergebnis >= Etage0)
        {
          motor.step(100, FORWARD, DOUBLE);
            US_Messen();
        }
            motor.step(0, FORWARD, DOUBLE);
      }
      else if(etage == 0 && !hoch)
      {
         Fahstuhl_Fahrt(hoch, 13);
         while(US_Ergebnis <= Etage0)
        {
          motor.step(100, BACKWARD, DOUBLE);
            US_Messen();
        }
            motor.step(0, FORWARD, DOUBLE);
      }
   
      US_Messen();
 
      ActualEtage = etage;

}


//-----------------------------------------------------------------------------



//------------------------------------------------------------------
void LCD_ScrollText()
{

  //check hoow long the programm has been started
  if ((millis() / 1000) <= 10)
  {
    // scroll 13 positions (string length) to the left
    // to move it offscreen left:
    for (int positionCounter = 0; positionCounter < 13; positionCounter++) {
      // scroll one position left:
      lcd.scrollDisplayLeft();
      // wait a bit:
      delay(150);
    }

    // scroll 29 positions (string length + display length) to the right
    // to move it offscreen right:
    for (int positionCounter = 0; positionCounter < 29; positionCounter++) {
      // scroll one position right:
      lcd.scrollDisplayRight();
      // wait a bit:
      delay(150);
    }

    // scroll 16 positions (display length + string length) to the left
    // to move it back to center:
    for (int positionCounter = 0; positionCounter < 16; positionCounter++) {
      // scroll one position left:
      lcd.scrollDisplayLeft();
      // wait a bit:
      delay(150);
    }

    // delay at the end of the full loop:
    delay(1000);
  }

}
//--------------------------------------------------------
