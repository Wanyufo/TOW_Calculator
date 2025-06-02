using System;
using UnityEngine;

namespace TOW_Calc_Full.Scripts
{
    public class UnitySimulationInterface : MonoBehaviour
    {
        private void Start()
        {
            _mockUnityScene = new MockUnityScene();
            Unit[] sideAUnits = _mockUnityScene.GetSideAUnits(); // Unit 1: 'A', Unit 2: 'B'
            Unit[] sideBUnits = _mockUnityScene.GetSideBUnits(); // Unit 1: 'C', Unit 2: 'D'

            // Combine both arrays into a single array
            Unit[] allUnits = new Unit[sideAUnits.Length + sideBUnits.Length];
            sideAUnits.CopyTo(allUnits, 0);
            sideBUnits.CopyTo(allUnits, sideAUnits.Length);
            _mockUnityScene.LogUnitPositions(allUnits);

        }

        Battle _battle;
        private SimulationManager _simulationManager;
        private MockUnityScene _mockUnityScene;
    
        public void InitializeBattle()
        {
            // get all the units etc form the scene and construct the Battle object
           
            _battle = _mockUnityScene.GetBattle();
            
        }
        
        public SimulationResult RunSimulation()
        {
            // check if the simulation manager and battle are initialized
            if (_simulationManager == null)
            {
                Debug.LogError("SimulationManager is not initialized.");
                return null;
            }
            if (_battle == null)
            {
                Debug.LogError("Battle is not initialized.");
                return null;
            }
            
            return _simulationManager.RunSimulation();
        }
    }
}
