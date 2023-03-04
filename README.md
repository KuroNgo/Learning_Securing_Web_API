# Quá trình học Securing Web API
Quá trình học Web-API-Securing 

* Day 1: Học sử dụng Swagger và Entity Framework (Code First)
* Day 2: 
    * Học Fluent API trong Entity Framework Core 
    * Controller Action Return Type & HTTP Status Code 
    * Repository Pattern in ASP.NET Core Web API + Secure API with Json Web Token (JWT)
* Day 3: Secure API with Json Web Token (JWT)
  * Json Web Tonken (JWT) là gì?
    * JSON Web Mã (JWT) là một chuẩn mở (RFC 7519) định nghĩa một cách nhỏ gọn và khép kín để truyền một cách an toàn thông tin giữa các bên dưới dạng đối tượng JSON. Thông tin này có thể được xác minh và đáng tin cậy vì nó có chứa chữ ký số. JWTs có thể được ký bằng một thuật toán bí mật (với thuật toán HMAC) hoặc một public / private key sử dụng mã hoá RSA.

    * Một ví dụ về JWT Token:
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjEzODY4OTkxMzEsImlzcyI6ImppcmE6MTU0ODk1OTUiLCJxc2giOiI4MDYzZmY0Y2ExZTQxZGY3YmM5MGM4YWI2ZDBmNjIwN2Q0OTFjZjZkYWQ3YzY2ZWE3OTdiNDYxNGI3MTkyMmU5IiwiaWF0IjoxMzg2ODk4OTUxfQ.uKqU9dTB6gKwG6jQCuXYAiMNdfNRw98Hw_IWuA5MaMo
    * Thoạt trông phức tạp là thế nhưng nếu hiểu, cấu trúc của một JWT chỉ đơn giản như sau:
      *Cấu trúc: "< base64-encoded header > . < base64-encoded payload > . < base64-encoded signature >"
    *Nói một cách khác, JWT là sự kết hợp (bởi dấu .) một Object Header dưới định dạng JSON được encode base64, một payload object dưới định dạng JSOn được encode base64 và một Signature cho URI cũng được mã hóa base64 nốt.
  * So sánh giữa Authentication và Authorization
  * Khai báo Authentication dùng JwtBearer
  * Cấu hình sinh Token
  * Demo gọi API sử dụng Authentication Header
  * Sử dụng Postman
