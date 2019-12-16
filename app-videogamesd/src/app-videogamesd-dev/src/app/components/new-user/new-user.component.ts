import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/User';
import { UserService } from 'src/app/services/user.service';
import { Game } from 'src/app/models/Game';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { sha256, sha224 } from 'js-sha256';

// import custom validator to validate that password and confirm password fields match
import { MustMatch } from '../../_helpers/must-match.validator';

@Component({
  selector: 'app-new-user',
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.css']
})
export class NewUserComponent implements OnInit {

  isChecked: boolean = false;
  isChecked1: boolean = false;
  users: User[];
  registerForm: FormGroup;
  submitted = false;
  formfull = false;
  name: string;
  username: string;
  password: string;
  email: string;
  init_date: Date = new Date();
  birthdate: Date;
  success: boolean;
  error: boolean;
  games: {
    planificacion: {
      subscribed: boolean;
      _score: {
        level0: {
          date: string[],
          score: number[]
        },
        level1: {
          date: string[],
          score: number[]
        },
        level2: {
          date: string[],
          score: number[]
        }
      }
    },
    dress: {
      subscribed: boolean;
      _score: {
        level0: {
          date: string[],
          score: number[]
        },
        level1: {
          date: string[],
          score: number[]
        },
        level2: {
          date: string[],
          score: number[]
        }
      }
    }
    total_score: number;
  };



  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder
  ) {
    this.userService.getUsers()
      .subscribe(users => {
        this.users = users;
      });

      this.registerForm = this.formBuilder.group({
        name: ['', Validators.required],
        username: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', Validators.required],
        confirmPassword: ['', Validators.required],
        birthdate: [null, Validators.required],
        games: this.formBuilder.group({
          planificacion: this.formBuilder.group({
            subscribed: true,
            bathroom: {
              _score0: {
                date: null,
                score: null,
                time:null
              },
              _score1: {
                date: null,
                score: null,
                time:null
              },
              _score2: {
                date: null,
                score: null,
                time:null
              }
            },
            dress: {
              _score0: {
                date: null,
                score: null,
                time:null
              },
              _score1: {
                date: null,
                score: null
              },
              _score2: {
                date: null,
                score: null,
                time:null
              }
            }
          }),
        })
      },
        {
          validator: MustMatch('password', 'confirmPassword')
        })
  }

  ngOnInit() {
    // console.log('entra en nuevo usuario');
 
  }

  uncheck(event) {
    this.isChecked = !this.isChecked;
  }

  uncheck1(event) {
    this.isChecked1 = !this.isChecked1;
  }

  addUser(event) {
    event.preventDefault();
    const user = this.registerForm.value;
    this.submitted = true;
    // stop here if form is invalid
    if (this.registerForm.invalid) {
      console.log('formulario inválido');
      return;
    } else if (!user) {
      console.log("Error!");
      this.error = true;
    } else {
      this.formfull = true;
      const userDa = {
        name: user.name,
        username: user.username,
        email: user.email,
        birthdate: user.birthdate,
        games: user.games,
        password: sha256(user.password),
        init_date: new Date(),
        total_score: 0,
        discriminator: '0000',
        status: 0,
        token: null,
        LastLogin: new Date(),
        activeConnection: 0
      }
      this.userService.addUser(userDa)
        .subscribe(user => {
          this.users.push(user);
        });
    }

    this.success = true;
    // alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.registerForm.value))
    this.registerForm.reset();
    this.registerForm.setErrors(null);
  }

  get f() {
    return this.registerForm.controls;
  }
}
