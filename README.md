## Fibonacci Calculator

Sample project where you can obtain the nth number in the [Fibonacci sequence](https://en.wikipedia.org/wiki/Fibonacci_number).

### Built With

* [.NET Core](https://dotnet.microsoft.com/en-us/)
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
* [Angular](https://angular.io/)
* [SQL Server](https://www.microsoft.com/en-us/sql-server)
* [Bootstrap](https://getbootstrap.com)

### Technology choices

I choose this stack because is what I feel most comfortable with, except Angular. I didn't worked with Angular these last few years so I gave it a shot.
For the interaction with the database I used EF Core and the Repository pattern alongside it, since it is always a good pattern to offer good encapsulation regarding database interaction.

### Calculating the nth number

There are various ways to calculate the nth number in the Fibonacci sequence, some more performant than others. 
Using recursion or even using a list to store the numbers already calculated is one approach, but not the more performant.
I knew there were more methods to calculate it so I started searching. 

The first one I tried was using the formula Fn = {[(√5 + 1)/2] ^ n} / √5 
It didn't worked because of floating point precision: when numbers were bigger, it would always return wrong result.

So I used Matrix exponentiation to make the calculation.