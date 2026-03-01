import { Component, effect, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  loginForm!: FormGroup;
  submitted = false;
  router = inject(Router);
  
  
  constructor(
    private formBuilder: FormBuilder,

  ) { 
    
  }

  ngOnInit(): void {
   
  }

}
