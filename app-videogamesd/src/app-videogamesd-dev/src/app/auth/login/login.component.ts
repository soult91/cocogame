import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { User } from 'src/app/models/User';
import { Form, FormsModule, FormGroup } from '@angular/forms';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private location: Location, private authService: AuthService, private router: Router, private route: ActivatedRoute) { }
  returnUrl: string;
  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/users';
  }

  onLogin(form): void{
    console.log('Login', form.value);
    this.authService.login(form.value).subscribe(res =>{
      this.router.navigateByUrl(this.returnUrl);
    })
  }
}
