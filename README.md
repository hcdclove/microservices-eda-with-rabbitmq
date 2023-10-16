Event Drivien Microservices App with RabbitMQ and SignalR Client

![Alt text](NOP.png)

Very minimalist implementation of an event driven micro service application with a RabbitMQ channel to handle high stream of noise  data sent by an n number of stations (producers) and validated by a SignalR validator consumed by a n number of subscribers.

Note: In a normal microsersevice eda implementation there wouild be global services as part of the overall architecture providing necessary loggin, monitoring, security, etc that has been ignore here to keep this EDA as minimalist as possible as the main idea is to demonstrate the use of a third party broket such as RabbitMQ, Kafka, etc.

Basic Requirements:

![Alt text](image.png)