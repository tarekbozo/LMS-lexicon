﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebServer.Models;
using WebServer.Models.LMS;
using WebServer.Repository;
using WebServer.ViewModels;
using Microsoft.AspNet.Identity;

namespace WebServer.Controllers
{
    //A controller for User information
    [Authorize(Roles = "Admin,Student,Teacher")]
    public class UsersAPIController : ApiController
    {
        UsersRepository repository = new UsersRepository();

        // GET: Users
        [OverrideAuthorization]
        [Authorize(Roles = "Admin")]
        public List<PartialUserVM> GetUsers()
        {
            // The user can't be deleted if:
            // - the edited user actually is the current user (can't delete oneself's account)
            // - the user is responsible for some courses
            // - the user takes part to any course
            // - the user has uploaded some documents (whatever purpose they have)
            // - the user has published some news

            return repository.Users().Select(u => new PartialUserVM
            {
                Id = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                BirthDate = u.BirthDate,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = repository.GetUserRole(u.Id).Name,
                IsDeletable = User.Identity.GetUserId() != u.Id &&
                              u.Courses.Count == 0 &&
                              u.Schedules.Count == 0 &&
                              u.Documents.Count == 0 &&
                              u.News.Count == 0
            }).ToList();
        }
        [OverrideAuthorization]
        [Authorize]
        public List<PartialUserVM> GetCompromisedUsers()
        {
            return repository.Users().Select(u => new PartialUserVM
            {
                UserName = u.UserName,
                Email = u.Email,
            }).ToList();
        }
        // GET: Students
        [OverrideAuthorization]
        [Authorize(Roles = "Admin")]
        public List<PartialUserVM> GetStudents()
        {
            return repository.Students().Select(s => new PartialUserVM
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                BirthDate = s.BirthDate,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber
            }).ToList();
        }

        public IHttpActionResult GetUserInfoFromCurrentUser(string userName)
        {
            if (userName == "" || userName == null)
            {
                return BadRequest();
            }

            User _user = repository.UserById(userName);
            if (_user == null)
            {
                _user = null;
                return NotFound();
            }
            else if (_user.UserName != userName && _user.Id != _user.Id)
            {
                _user = null;
                return BadRequest();
            }
            string _userRole = "Unknown";
            if (userName != _user.Id)
            {
                if (User.IsInRole("Student")) { _userRole = "Student"; }
                else if (User.IsInRole("Teacher")) { _userRole = "Teacher"; }
                else if (User.IsInRole("Admin")) { _userRole = "Admin"; }
                else
                {
                    return InternalServerError();
                }
            }
            else
            {
                Role r = repository.GetUserRole(_user.Id);
                if (r.Name == "Student") { _userRole = "Student"; }
                else if (r.Name == "Teacher") { _userRole = "Teacher"; }
                else if (r.Name == "Admin") { _userRole = "Admin"; }
                else
                {
                    r = null;
                    return InternalServerError();
                }
                r = null;
            }
            //Everything went perfect so let's send user info
            return Ok(new PartialUserVM
            {
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                Role = _userRole,
                Id = _user.Id,
                PhoneNumber = _user.PhoneNumber,
                Email = _user.Email,
                BirthDate = _user.BirthDate,
                UserName = _user.UserName
            });




        }

        [OverrideAuthorization]
        [Authorize(Roles = "Admin")]
        public List<PartialUserVM> GetAvailableTeachers(int subjectID)
        {
            return repository.AvailableTeachers(subjectID).Select(t => new PartialUserVM
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName
            }).ToList();
        }
        [OverrideAuthorization]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IHttpActionResult GetRoles()
        {
            List<Role> roles = new List<Role>();
            foreach (Role r in new RolesRepository().Roles())
            {
                Role temp = new Role();
                temp.Id = r.Id;
                temp.Name = r.Name;
                roles.Add(temp);
            }
            if (roles.Count == 0) { return NotFound(); }
            return Ok(roles);
        }
    }
}