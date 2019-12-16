import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from 'src/app/models/User';
import { Game } from 'src/app/models/Game';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Subject } from 'rxjs';


@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  users: User[];
  name: string;
  username: string;
  password: string;
  email: string;
  init_date: Date;
  birthdate: Date;
  games: {
    planificacion: {
      subscribed: boolean;
      score: number;
    },
    memoria: {
      subscribed: boolean;
      score: number;
    }
  };
  total_score: number;
  n?: number;
  last_time: number;
  constructor(private userService: UserService,
    private location: Location) {
    this.userService.getUsers()
      .subscribe(users => {
        console.log(users);
        this.users = users;
        this.dtTrigger.next();
      });
  }


ngOnInit() {
  // console.log('entra en usuarios');
  this.dtOptions = {
    pagingType: 'full_numbers',
    pageLength: 5,
    processing: true,
    columnDefs: [ {
      targets: 4,
      orderable: false
      },
      {
        targets: 5,
        orderable: false
        },
     ]
  };
}


deleteUser(id){
  const res = confirm("¿Estás seguro de que quieres eliminar a este usuario?")
  if (res) {
    const users = this.users;
    this.userService.deleteUser(id)
      .subscribe(data => {
        if (data.n == 1) {
          for (let i = 0; i < users.length; i++) {
            if (users[i]._id == id) {
              this.users.splice(i, 1);
            }
          }
        }
      });
  }
  return;
}

isStringDate(date){
  if (typeof date == 'string' && date.length > 10) {
    return true;
  }
  return false;
}

updateUser(user: User){
  const updUser = {
    _id: user._id,
    name: user.name,
    username: user.username,
    password: user.password,
    email: user.email,
    init_date: user.init_date,
    birthdate: user.birthdate,
    games: user.games
  }

  this.userService.updateUser(updUser)
    .subscribe(res => {
      console.log(res);
    })
}

showInfo(id){
  console.log(id);
  this.location.go('/users/', id);
}

}
