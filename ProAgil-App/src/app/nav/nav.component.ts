import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(
    public router: Router
  , public authService: AuthService
  , private toastr: ToastrService
  ) {
  }

  ngOnInit() {
  }

  entrar() {
    this.router.navigate(['/user/login']);
  }

  loggedIn() {
    console.log(this.authService.loggedIn());
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    this.toastr.show('Log Out');
    this.router.navigate(['/user/login']);
  }

  userName() {
    return sessionStorage.getItem('username');
  }
}
