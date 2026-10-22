Use GestaoFrota;

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id 
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'admin@gestaofrota.com' 
  AND r.Name = 'Admin';