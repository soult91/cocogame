import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute } from "@angular/router";
import { User } from 'src/app/models/User';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  id: string;
  user: User;
  users: Observable<string[]>;
  currentUser: string;

  score: Array<any> = new Array();
  dates: Array<any> = new Array();

  scoresBath: Array<any> = new Array();
  datesBath: Array<any> = new Array();

  scoresBath1: Array<any> = new Array();
  datesBath1: Array<any> = new Array();

  scoresBath2: Array<any> = new Array();
  datesBath2: Array<any> = new Array();

  scoresDress: Array<any> = new Array();
  datesDress: Array<any> = new Array();

  scoresDress1: Array<any> = new Array();
  datesDress1: Array<any> = new Array();

  scoresDress2: Array<any> = new Array();
  datesDress2: Array<any> = new Array();

  LineChartBath = [];
  LineChartBath1 = [];
  LineChartBath2 = [];
  LineChartDress = [];
  LineChartDress1 = [];
  LineChartDress2 = [];

  bathroom : boolean;
  dress : boolean;
  bathLvl0 : boolean;
  bathLvl1 : boolean;
  bathLvl2 : boolean;
  dressLvl0 : boolean;
  dressLvl1 : boolean;
  dressLvl2 : boolean;

  private _userSub: Subscription;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute) { }

  ngOnInit() {

    
    this.route.paramMap.subscribe(params => {
      this.id = params.get("id");
    });

    this.userService.getUser(this.id)
      .subscribe(user => {
        console.log(user);
        //  this.user aqui se carga con todos los datos del user que ha filtrado
        // asi que en la vista solo hay que cargar los datos llamandolos asi 'user.parametro'
        this.user = user;

      //  puntuaciones y fechas del juego baño level0
        if (user.games.planificacion.bathroom._score0.score != null) {
          for (var i = 0; i < user.games.planificacion.bathroom._score0.score.length; i++) {
            this.scoresBath.push(user.games.planificacion.bathroom._score0.time[i]);
            this.datesBath.push(user.games.planificacion.bathroom._score0.date[i]);

          }
        }

        //puntuaciones y fechas del juego baño level1
        if (user.games.planificacion.bathroom._score1.score != null) {
          for (var i = 0; i < user.games.planificacion.bathroom._score1.score.length; i++) {
            this.scoresBath1.push(user.games.planificacion.bathroom._score1.time[i]);
            this.datesBath1.push(user.games.planificacion.bathroom._score1.date[i]);

          }
        }

        //puntuaciones y fechas del juego baño level2
        if (user.games.planificacion.bathroom._score2.score != null) {
          for (var i = 0; i < user.games.planificacion.bathroom._score2.score.length; i++) {
            this.scoresBath2.push(user.games.planificacion.bathroom._score2.time[i]);
            this.datesBath2.push(user.games.planificacion.bathroom._score2.date[i]);

          }
        }

        //puntuaciones y fechas del juego vestidor level0

        if (user.games.planificacion.dress._score0.score != null) {
          for (var i = 0; i < user.games.planificacion.dress._score0.score.length; i++) {
            this.scoresDress.push(user.games.planificacion.dress._score0.time[i]);
            this.datesDress.push(user.games.planificacion.dress._score0.date[i]);

          }
        }
        //puntuaciones y fechas del juego vestidor level1
        if (user.games.planificacion.dress._score1.score != null) {
          for (var i = 0; i < user.games.planificacion.dress._score1.score.length; i++) {
            this.scoresDress1.push(user.games.planificacion.dress._score1.time[i]);
            this.datesDress1.push(user.games.planificacion.dress._score1.date[i]);

          }
        }

        //puntuaciones y fechas del juego vestidor level2
        if (user.games.planificacion.dress._score2.score != null) {
          for (var i = 0; i < user.games.planificacion.dress._score2.score.length; i++) {
            this.scoresDress2.push(user.games.planificacion.dress._score2.time[i]);
            this.datesDress2.push(user.games.planificacion.dress._score2.date[i]);

          }
        }

        Chart.defaults.global.maintainAspectRatio = false;
        // Graph Bathroom level0
        if(this.datesBath != null){
          this.LineChartBath = new Chart('canvas', {
            type: 'line',
            data: {
              labels: this.datesBath,
  
              datasets: [
                {
                  data: this.scoresBath,
                  borderColor: '#323366',
                  // backgroundColor: "#0000FF",
                }
              ]
            },
            options: {
              maintainAspectRatio: false,
              legend: {
                display: false
              },
              scales: {
                xAxes: [{
                  display: true,
                  ticks:{
                    fontSize:18,
                    rotation: 90,
                  },
                  scaleLabel: {
                    display: true,
                    labelString: 'Fecha',
                    fontSize: 19
                  }
                }],
                yAxes: [{
                  display: true,
                  ticks:{
                    fontSize:18,
                    rotation: 45,
                  },
                  scaleLabel: {
                    display: true,
                    labelString: 'Tiempo (s)',
                    fontSize: 19
                  }
                }],
              }
            }
          });
        }
       
        //Graph Bathroom level1
         if(this.datesBath1 != null){
           this.LineChartBath1 = new Chart('canvasBath1', {
             type: 'line',
             width: 600,
             height: 300,
             data: {
               labels: this.datesBath1,
  
               datasets: [
                 {
                   data: this.scoresBath1,
                   borderColor: '#3cb371',
                   backgroundColor: "#0000FF",
                 }
               ]
             },
             options: {
               maintainAspectRatio: false,
               legend: {
                 display: false
               },
               scales: {
                 xAxes: [{
                   display: true,
                   ticks:{
                     fontSize: 30
                   }
                 }],
                 yAxes: [{
                   display: true,
                   ticks:{
                     fontSize: 20
                   }
                 }],
               }
             }
           });
         }
     

        //  Graph Bathroom level2
         if(this.datesBath2 != null){
           this.LineChartBath2 = new Chart('canvasBath2', {
             type: 'line',
             width: 600,
             height: 300,
             data: {
               labels: this.datesBath2,
  
               datasets: [
                 {
                   data: this.scoresBath2,
                   borderColor: '#3cb371',
                   backgroundColor: "#0000FF",
                 }
               ]
             },
             options: {
               maintainAspectRatio: false,
               legend: {
                 display: false
               },
               scales: {
                 xAxes: [{
                   display: true
                 }],
                 yAxes: [{
                   display: true
                 }],
               }
             }
           });
         }
 

        //  Graph Dress level0
         if(this.datesDress != null){
           this.LineChartDress = new Chart('canvasDress0', {
             type: 'line',
             width: 600,
             height: 300,
             data: {
               labels: this.datesDress,
  
               datasets: [
                 {
                   data: this.scoresDress,
                   borderColor: '#3cb371',
                   backgroundColor: "#0000FF",
                 }
               ]
             },
             options: {
               maintainAspectRatio: false,
               legend: {
                 display: false
               },
               scales: {
                 xAxes: [{
                   display: true
                 }],
                 yAxes: [{
                   display: true
                 }],
               }
             }
           });
         }


          // Graph Dress level1
         if(this.datesDress1 != null) {
           this.LineChartDress1 = new Chart('canvasDress1', {
             type: 'line',
             width: 600,
             height: 300,
             data: {
               labels: this.datesDress1,
  
               datasets: [
                 {
                   data: this.scoresDress1,
                   borderColor: '#3cb371',
                   backgroundColor: "#0000FF",
                 }
               ]
             },
             options: {
               maintainAspectRatio: false,
               legend: {
                 display: false
               },
               scales: {
                 xAxes: [{
                   display: true
                 }],
                 yAxes: [{
                   display: true
                 }],
               }
             }
           });
  
         }
 
          // Graph Dress level2
         if(this.datesDress2 != null){
           this.LineChartDress2 = new Chart('canvasDress2', {
             type: 'line',
             width: 600,
             height: 300,
             data: {
               labels: this.datesDress2,
  
               datasets: [
                 {
                   data: this.scoresDress2,
                   borderColor: '#3cb371',
                   backgroundColor: "#0000FF",
                 }
               ]
             },
             options: {
               maintainAspectRatio: false,
               legend: {
                 display: false
               },
               scales: {
                 xAxes: [{
                   display: true
                 }],
                 yAxes: [{
                   display: true
                 }],
               }
             }
           });
         }
     

      });
    
  }

  levelsBathroom() {
    if (this.bathroom) {
      this.bathroom = false;
      this.dress = false;
    } else {
      this.bathroom = true;
      this.dress = false;
    }
  }

  levelsDress() {
    if (this.dress) {
      this.dress = false;
      this.bathroom = false;
    } else {
      this.dress = true;
      this.bathroom = false;
    }
  }

  activeBathLvl0() {
    this.bathLvl0 = true;
    this.bathLvl1 = false;
    this.bathLvl2 = false;
  }

  activeBathLvl1() {
    this.bathLvl0 = false;
    this.bathLvl1 = true;
    this.bathLvl2 = false;
  }

  activeBathLvl2() {
    this.bathLvl0 = false;
    this.bathLvl1 = false;
    this.bathLvl2 = true;
  }

  activeDressLvl0() {
    this.dressLvl0 = true;
    this.dressLvl1 = false;
    this.dressLvl2 = false;
  }

  activeDressLvl1() {
    this.dressLvl0 = false;
    this.dressLvl1 = true;
    this.dressLvl2 = false;
  }

  activeDressLvl2() {
    this.dressLvl0 = false;
    this.dressLvl1 = false;
    this.dressLvl2 = true;
  }


}


