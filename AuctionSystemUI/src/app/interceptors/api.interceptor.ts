import { HttpInterceptorFn } from '@angular/common/http';

export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  const baseUrl = 'http://localhost:5001/api'; 
  const apiReq = req.clone({
    url: `${baseUrl}/${req.url}`
  });

  return next(apiReq);
};