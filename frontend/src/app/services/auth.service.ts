import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environments';

export interface RegisterRequest {
  clerkUserId: string;
  email: string;
  organizationName: string;
  clerkOrgId: string;
}

export interface RegisterResponse {
  message: string;
  userId: number;
  orgId: string;
  userActive: boolean;
  orgActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}`; // Ajusta el puerto si es diferente

  constructor(private http: HttpClient) {}

  registerUser(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/auth/register`, request);
  }
}