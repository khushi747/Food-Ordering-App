import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { LoginService } from '../services/login.service';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRouteSnapshot } from '@angular/router';
import { of } from 'rxjs';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let loginService: jasmine.SpyObj<LoginService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(() => {
    const loginServiceSpy = jasmine.createSpyObj('LoginService', [
      'getToken',
      'getUserRole',
    ]);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        AuthGuard,
        { provide: LoginService, useValue: loginServiceSpy },
        { provide: Router, useValue: routerSpy },
      ],
    });

    guard = TestBed.inject(AuthGuard);
    loginService = TestBed.inject(LoginService) as jasmine.SpyObj<LoginService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should allow access if a valid token exists', () => {
    loginService.getToken.and.returnValue('valid_token');
    loginService.getUserRole.and.returnValue('admin');

    const route = new ActivatedRouteSnapshot();
    expect(guard.canActivate(route)).toBeTrue();
  });

  it('should deny access and redirect to login if no token is found', () => {
    loginService.getToken.and.returnValue(null);

    const route = new ActivatedRouteSnapshot();
    expect(guard.canActivate(route)).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should deny access and redirect if user role does not match required role', () => {
    loginService.getToken.and.returnValue('valid_token');
    loginService.getUserRole.and.returnValue('user');

    const route = new ActivatedRouteSnapshot();
    (route.data as any) = { role: 'admin' }; // Mock required role

    expect(guard.canActivate(route)).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });
});
