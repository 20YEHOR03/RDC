#include <SPI.h>
#include <Ethernet.h>
#include "DHT.h"
#define DHTPIN 2     
#define DHTTYPE DHT11

// replace the MAC address below by the MAC address printed on a sticker on the Arduino Shield 2
byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
DHT dht(DHTPIN, DHTTYPE);

EthernetClient client;

int    HTTP_PORT   = 65202;
String HTTP_METHOD = "GET"; // or POST
char   HOST_NAME[] = "192.168.0.123";
String PATH_NAME   = "/api/Weather";
byte server[] = { 192, 168, 0, 123 };
void setup() {
  Serial.begin(9600);
  dht.begin();

  // initialize the Ethernet shield using DHCP:
  if (Ethernet.begin(mac) == 0) {
    Serial.println("Failed to obtaining an IP address using DHCP");
    //while(true);
  }
  Serial.println(Ethernet.localIP());


  // connect to web server on port 80:
  if(client.connect(HOST_NAME, HTTP_PORT)) {
    // if connected:
    Serial.println("Connected to server");
    // make a HTTP request:
    // send HTTP header
    double temp = dht.readTemperature();
    double humi = dht.readHumidity();
    String queryString = String("?temperature=") + String(temp) + String("&humidity=") + String(humi);
    Serial.println(HTTP_METHOD + " " + PATH_NAME + queryString +" HTTP/1.1");
    client.println(HTTP_METHOD + " " + PATH_NAME + queryString +" HTTP/1.1");
    client.println("Host: http://" + String(HOST_NAME));
    Serial.println("Host: " + String(HOST_NAME));
    client.println("Connection: close");
    client.println(); // end HTTP header

    while(client.connected()) {
      if(client.available()){
        // read an incoming byte from the server and print it to serial monitor:
        char c = client.read();
        Serial.print(c);
      }
    }

    // the server's disconnected, stop the client:
    client.stop();
    Serial.println();
    Serial.println("disconnected");
  } else {// if not connected:
    Serial.println("connection failed");
    float h = dht.readHumidity();
    float t = dht.readTemperature();

    // перевіряємо чи правильні дані отримали
    if (isnan(t) || isnan(h)) {
      Serial.println("Error reading from DHT");
    } else {
      Serial.print("Humidity: "); 
      Serial.print(h);
      Serial.print(" %t");
      Serial.print("Temperature: "); 
      Serial.print(t);
      Serial.println(" *C");
    }
  }
  
}

void loop() {

}