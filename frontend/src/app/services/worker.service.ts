import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environments';

export interface Worker {
  id?: number;
  name: string;
  numero:number;
  role: string;
  country: string;
  correo: string;
}

@Injectable({
  providedIn: 'root'
})
export class WorkerService {
  private apiUrl = `${environment.apiUrl}/Worker`; // cambia al endpoint de tu backend

  constructor(private http: HttpClient) {}

  getWorkers(): Observable<Worker[]> {
    return this.http.get<Worker[]>(this.apiUrl);
  }

  createWorker(worker: Worker): Observable<Worker> {
    return this.http.post<Worker>(this.apiUrl, worker);
  }

  updateWorker(worker: Worker): Observable<Worker> {
    return this.http.put<Worker>(`${this.apiUrl}/${worker.id}`, worker);
  }
}
