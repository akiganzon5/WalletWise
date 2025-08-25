import { Routes } from '@angular/router';
import { AuthLandingComponent } from './pages/auth-landing/auth-landing.component';
import { MainWalletComponent } from './pages/main-wallet/main-wallet.component';
import { CategoryListsComponent } from './pages/category-lists/category-lists.component';
import { WalletListsComponent } from './pages/wallet-lists/wallet-lists.component';
import { AuthGuard } from './interceptor/auth.guard';
import { AnalyticsComponent } from './pages/analytics/analytics.component';

export const routes: Routes = [
  { path: '', component: AuthLandingComponent },
  { path: 'wallet', component: MainWalletComponent, canActivate: [AuthGuard] },
  { path: 'categories', component: CategoryListsComponent, canActivate: [AuthGuard] },
  { path: 'all/wallets', component: WalletListsComponent, canActivate: [AuthGuard] },
  { path: 'analytics', component: AnalyticsComponent, canActivate: [AuthGuard] },
];
