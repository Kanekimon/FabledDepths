﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CardManager : Singleton<CardManager>
{
    Player _player;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
        
    }

}

