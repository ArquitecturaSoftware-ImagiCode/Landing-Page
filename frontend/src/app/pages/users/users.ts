import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Worker, WorkerService } from '../../services/worker.service';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './users.html',
  styleUrls: ['./users.css']
})
export class UsersComponent implements OnInit {
  workers: Worker[] = [];
  newWorker: Worker = {
    id: 0,
    numero: 0,
    name: '',
    role: '',
    country: '',
    correo: ''
  };
  editingIndex: number | null = null;

  constructor(private workerService: WorkerService) {}

  ngOnInit() {
    this.loadWorkers();
  }

  loadWorkers() {
    this.workerService.getWorkers().subscribe(data => {
      this.workers = data;
    });
  }

  // Crear nuevo worker desde la fila de registro
  createWorker() {
    if (
      !this.newWorker.name ||
      !this.newWorker.role ||
      !this.newWorker.country ||
      !this.newWorker.numero ||
      !this.newWorker.correo
    ) {
      alert('Todos los campos son obligatorios.');
      return;
    }

    this.workerService.createWorker(this.newWorker).subscribe(created => {
      this.workers.push(created);
      // Limpiar inputs
      this.newWorker = { id: 0, numero: 0, name: '', role: '', country: '', correo: '' };
    });
  }

  // Comenzar edición en línea
  startEditing(index: number) {
    this.editingIndex = index;
  }

  // Guardar cambios en un worker existente
  saveWorker(worker: Worker) {
    if (!worker.name || !worker.role || !worker.country || !worker.numero || !worker.correo) {
      alert('Todos los campos son obligatorios.');
      return;
    }

    this.workerService.updateWorker(worker).subscribe(result => {
      const index = this.workers.findIndex(w => w.id === result.id);
      if (index > -1) this.workers[index] = result;
      this.editingIndex = null;
    });
  }

  // Cancelar edición
  cancelEditing() {
    this.editingIndex = null;
    this.loadWorkers(); // recarga para descartar cambios
  }
}
