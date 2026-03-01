import { Component, effect, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DashboardStore } from '../store/dashboard.store';

@Component({
  selector: 'app-dashboard-details',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './details.component.html',
  styleUrl: './details.component.scss'
})
export class DetailsComponent  {
  loginForm!: FormGroup;
  submitted = false;
  router = inject(Router);
  dashboardStore = inject(DashboardStore);
  
  
  constructor(
  ) { 
    this.dashboardStore.loadDiagItems();
  }
}
