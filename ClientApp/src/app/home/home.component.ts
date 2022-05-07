import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent {
  public currentCount: string;
  public numberToCalculate: number;
  public message: string;
  public errorMessage: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  callBackend() {
    if (this.numberToCalculate == null) {
      this.errorMessage = 'Please enter a number';
      return;
    }
    if (this.numberToCalculate < 1) {
      this.errorMessage = 'Please enter a number greater than 0';
      return;
    }
    if (this.numberToCalculate > 300) {
      this.errorMessage = 'Please enter a number less than 300';
      return;
    }
    this.errorMessage = null;
    this.message = "Calculating...";
    this.http.get(this.baseUrl + 'fibonacci/' + this.numberToCalculate, { responseType: 'text'})
      .subscribe(result => {
        this.currentCount = result;
        this.message = "The answer for entry " + this.numberToCalculate + " of the Fibonacci sequence is " + result;
        this.numberToCalculate = null;
      }, error => console.error(error));
  }
}
