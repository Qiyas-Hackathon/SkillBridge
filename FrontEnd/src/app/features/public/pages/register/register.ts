import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
type AccountType = 'candidate' | 'company';

@Component({
  imports: [RouterLink],
  standalone: true,
  selector: 'app-register',
  styleUrl: './register.css',
  templateUrl: './register.html',
})


export class Register {

  accountType: AccountType = 'candidate';

  selectAccountType(type: AccountType): void {
    this.accountType = type;
  }
}
