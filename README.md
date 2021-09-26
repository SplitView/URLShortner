
# URLShortner
How to run:
-- Use visual studio with asp.net core 5.0 installed or
-- Use command 

    docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d


Goto http://localhost:5000/swagger/index.html

-- Try on Post with version "1" and below payload for different scenarios:

- Default short url generate

    {
      "originalURL": "https://www.google.com/",
      "useCustomKey": false,
      "customUniqueKey": "string",
      "useCustomExpiry": false,
      "expiryTimeInSeconds": 0
    }

- Generate with custom key


    {
      "originalURL": "https://www.google.com/",
      "useCustomKey": true,
      "customUniqueKey": "googl",
      "useCustomExpiry": false,
      "expiryTimeInSeconds": 0
    }


- Generate with custom expiry time in seconds

    {
      "originalURL": "https://www.google.com/",
      "useCustomKey": false,
      "customUniqueKey": "string",
      "useCustomExpiry": true,
      "expiryTimeInSeconds": 999999
    }

*Response:*

    {
      "id": "61507b319f13c8c5d516bab3",
      "originalURL": "https://www.google.com/",
      "uniqueKey": "riHZgk",
      "expiryDate": "2021-10-08T03:39:28.7489403Z",
      "redirectURL": "http://localhost:5000/riHZgk"
    }

-- Go to redirectURL in browser to get redirected to original url.

-- **To get redirect count goto get method: and enter version as "1" and uniqueKey as given in previous response or second part of redirect url**.

Tools used:
-- Redis for cache,
-- Mongo for persistence,
-- RabbitMQ for events(Save redirect log for more faster response)
