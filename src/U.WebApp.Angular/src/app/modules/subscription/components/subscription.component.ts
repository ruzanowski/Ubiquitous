import {Component, OnInit} from '@angular/core';
import {SubscriptionService} from "../subscription.service";
import {Preferences} from "../models/preferences.model";

@Component({
  selector: 'dashboard-subscription',
  templateUrl: './subscription.component.html',
  styleUrls: ['./subscription.component.css']
})

export class SubscriptionComponent
{
  preferences: Preferences;

  constructor(private subscription: SubscriptionService)
  {
    this.subscription.getMyPreferences().subscribe(preferences => this.preferences = preferences);
  }

}





