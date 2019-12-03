import { Injectable } from '@angular/core';
import { ToastrService, IndividualConfig } from 'ngx-toastr';

@Injectable()
export class ReactiveToasterService {

  options: IndividualConfig;

  constructor(
    private toastr: ToastrService
  ) {
    this.options = this.toastr.toastrConfig;
    this.options.positionClass = 'toast-top-right';
    this.options.timeOut = 2500;
  }

  showToast(title, message, type) {
    this.toastr.show(message, title, this.options, 'toast-' + type);
  }

}
