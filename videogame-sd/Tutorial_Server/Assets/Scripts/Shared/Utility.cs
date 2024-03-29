﻿using System.Text.RegularExpressions;
using System;
using System.Security.Cryptography;
using System.Linq;
using System.Text;

public static class Utility
{
    public const string EMAIL_PATTERN = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
    public const string USERNAME_AND_DISCRIMINATOR_PATTERN = @"^[a-zA-Z0-9]{4,20}#[0-9]{4}$";
    public const string USERNAME_PATTERN = @"^[a-zA-Z0-9]{4,20}$";
    public const string RANDOM_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static bool IsEmail(string email)
    {
        if(email !=null)
            return Regex.IsMatch(email, EMAIL_PATTERN);

        else
            return false;
        
    }

    public static bool IsUsername(string username)
    {
        if(username !=null)
            return Regex.IsMatch(username, USERNAME_PATTERN);

        else
            return false;
    }

    public static bool IsUsernameAndDiscriminator(string username)
    {
        if(username !=null)
            return Regex.IsMatch(username, USERNAME_AND_DISCRIMINATOR_PATTERN);

        else
            return false;
    }

    public static string GenerateRandom(int length)
    {
        Random r = new Random();
        return new string(Enumerable.Repeat(RANDOM_CHARS, length).Select(s => s[r.Next(s.Length)]).ToArray());
    }

    public static string Sha256FromString(string toEncrypt)
    {
        var message = Encoding.UTF8.GetBytes(toEncrypt);
        SHA256Managed hashString = new SHA256Managed();
        // igual hay que cambiar SHA256MAnaged por Sha256FromString

        string hex = "";
        var hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
            hex += String.Format("{0:x2}", x);

        return hex;
    }

    
}