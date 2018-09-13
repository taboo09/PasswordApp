import { Routes } from '@angular/router';
import { MenuComponent } from './menu/menu.component';
import { UserInfoComponent } from './user-info/user-info.component';
import { ListPasswordsComponent } from './list-passwords/list-passwords.component';
import { NewPasswordComponent } from './new-password/new-password.component';
import { AboutComponent } from './about/about.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
    { path: 'home', component: HomeComponent },
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'menu', component: MenuComponent },
            { path: 'userinfo', component: UserInfoComponent },
            { path: 'passwords', component: ListPasswordsComponent },
            { path: 'new', component: NewPasswordComponent },
        ]
    },
    { path: 'about', component: AboutComponent },
    { path: '**', redirectTo: 'home', pathMatch: 'full' }
]
