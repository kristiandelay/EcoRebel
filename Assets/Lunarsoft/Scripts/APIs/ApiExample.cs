using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Lunarsoft
{

    [Serializable]
    public struct RandomUserApiResponse
    {
        public Result[] results;
        public Info info;
    }

    [Serializable]
    public struct Result
    {
        public string gender;
        public Name name;
        public Location location;
        public string email;
        public Login login;
        public Dob dob;
        public Registered registered;
        public string phone;
        public string cell;
        public Id id;
        public Picture picture;
        public string nat;
    }

    [Serializable]
    public struct Name
    {
        public string title;
        public string first;
        public string last;
    }

    [Serializable]
    public struct Location
    {
        public Street street;
        public string city;
        public string state;
        public string country;
        public string postcode;
        public Coordinates coordinates;
        public Timezone timezone;
    }

    [Serializable]
    public struct Street
    {
        public int number;
        public string name;
    }

    [Serializable]
    public struct Coordinates
    {
        public string latitude;
        public string longitude;
    }

    [Serializable]
    public struct Timezone
    {
        public string offset;
        public string description;
    }

    [Serializable]
    public struct Login
    {
        public string uuid;
        public string username;
        public string password;
        public string salt;
        public string md5;
        public string sha1;
        public string sha256;
    }

    [Serializable]
    public struct Dob
    {
        public string date;
        public int age;
    }

    [Serializable]
    public struct Registered
    {
        public string date;
        public int age;
    }

    [Serializable]
    public struct Id
    {
        public string name;
        public string value;
    }

    [Serializable]
    public struct Picture
    {
        public string large;
        public string medium;
        public string thumbnail;
    }

    [Serializable]
    public struct Info
    {
        public string seed;
        public int results;
        public int page;
        public string version;
    }

}