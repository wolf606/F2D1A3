using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GameNetworking;
using JwtHandler;

namespace ApiHandler
{

    [System.Serializable]
    public struct LoginData
    {
        public string email;
        public string password;
    }

    [System.Serializable]
    public class AccessToken
    {
        public string status;
        public string accessToken;
    }

    [System.Serializable]
    public class ActivityToken
    {
        public string status;
        public string activityToken;
    }

    [System.Serializable]
    public class User
    {
        public string status;
        public UserData data;
    }

    [System.Serializable]
    public class UserData
    {
        public string id;
        public string email;
        public bool active;
        public List<string> role;
        public Profile profile;
        public string createdAt;
        public string updatedAt;
    }

    [System.Serializable]
    public class Profile
    {
        public string pro_nombre;
        public string pro_apelli;
        public string pro_tipide;
        public string pro_numide;
        public string pro_celpai;
        public string pro_celula;
        public string pro_avatar;
        public Address pro_addres;
    }

    [System.Serializable]
    public class Address
    {
        public string add_addres;
        public string add_city;
        public string add_poscod;
        public string add_countr;
        public string add_state;
        public string add_telcou;
        public string add_teleph;
    }

    [System.Serializable]
    public class Entity
    {
        public string status;
        public List<EntityData> data;
    }

    [System.Serializable]
    public class EntityData
    {
        public string id;
        public string ent_nombre;
        public Address ent_direcc;
        public string ent_celpai;
        public string ent_celula;
        public string ent_avatar;
    }

    [System.Serializable]
    public class Admission
    {
        public string status;
        public string message;
        public List<AdmissionData> data;
    }

    [System.Serializable]
    public class AdmissionData
    {
        public string id;
        public UserData patient;
        public string adm_patien;
        public string adm_profes;
        public string adm_entity;
        public string adm_admdat;
        public string adm_disdat;
        public Companion adm_compan;
        public string createdAt;
        public string updatedAt;
    }

    [System.Serializable]
    public class Companion
    {
        public Profile com_profil;
        public string com_parent;
    }

    [System.Serializable]
    public class F2D1A3Resume
    {
        public int sco_lifes;
        public int sco_flies;
    }

    [System.Serializable]
    public class ScoreBoard
    {
        public string sco_start;
        public string sco_end;
        public int sco_score;
        public bool sco_win;
        public F2D1A3Resume sco_resume;
    }

    public static class ApiRequest
    {
        private static string apiUrl = "https://probauam-backend-production-4fda.up.railway.app/api/v1";

        public static IEnumerator Login(string email, string password, OnSuccess<AccessToken> onSuccess, OnError onError)
        {
            LoginData loginData = new LoginData{
                email = email,
                password = password
            };
            string jsonBody = JsonUtility.ToJson(loginData);
            yield return HttpRequest.Post<AccessToken>(apiUrl + "/login", jsonBody, null, onSuccess, onError);
        }

        public static IEnumerator GetLoggedInUser(string token, OnSuccess<User> onSuccess, OnError onError)
        {
            yield return HttpRequest.Get<User>(apiUrl + "/users/me", token, onSuccess, onError);
        }

        public static IEnumerator GetAvatar (string avatarUrl, OnSuccess<Sprite> onSuccess, OnError onError)
        {
            yield return HttpRequest.GetImage(avatarUrl, onSuccess, onError);
        }

        public static IEnumerator GetEntities (string token, OnSuccess<List<EntityData>> onSuccess, OnError onError)
        {
            JwtPayload payload = Jwt.GetPayload(token);
            yield return HttpRequest.Get<Entity>(apiUrl + "/users/" + payload.id + "/entities?active=true&cargo=employee", token, (Entity result) => {
                onSuccess(result.data);
            }, onError);
        }

        public static IEnumerator GetAdmissions (string token, string entityId, OnSuccess<List<AdmissionData>> onSuccess, OnError onError)
        {
            JwtPayload payload = Jwt.GetPayload(token);
            yield return HttpRequest.Get<Admission>(apiUrl + "/entities/" + entityId + "/professional/" + payload.id + "/admissions?active=true", token, (Admission result) => {
                onSuccess(result.data);
            }, onError);
        }

        public static IEnumerator GetActivityToken (string token, string activityId, string admissionId, OnSuccess<ActivityToken> onSuccess, OnError onError)
        {
            JwtPayload payload = Jwt.GetPayload(token);
            yield return HttpRequest.Get<ActivityToken>(apiUrl + "/activities/" + activityId + "/admissions/" + admissionId + "/token", token, onSuccess, onError);
        }

        public static IEnumerator SaveScore (string activityToken, string activityId, ScoreBoard scoreBoard, OnSuccess<ScoreBoard> onSuccess, OnError onError)
        {
            string jsonBody = JsonUtility.ToJson(scoreBoard);
            yield return HttpRequest.Post<ScoreBoard>(apiUrl + "/activities/" + activityId + "/scoreboards", jsonBody, activityToken, onSuccess, onError);
        }
    }
}