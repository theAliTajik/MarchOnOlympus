using UnityEngine;
using System.Collections;
using System;


    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnFixedUpdate();
        void OnExit();

    }

    public class StateMachine
    {
        private int m_currentState = -1;
        private int m_NextState = -1;
        private int m_lastState = -1;

        private bool m_debug = false;

        public delegate void Function();

        Function[] m_onEnterFunc;
        Function[] m_onExitFunc;
        Function[] m_onUpdateFunc;
        Function[] m_onFixedUpdateFunc;

        float m_time = 0;
        float m_timeFixed = 0;

        public StateMachine(Enum nStates) : this(Convert.ToInt32(nStates))
        {
        }

        public StateMachine(int nStates, bool debug = false)
        {
            m_onEnterFunc = new Function[nStates];
            m_onExitFunc = new Function[nStates];
            m_onUpdateFunc = new Function[nStates];
            m_onFixedUpdateFunc = new Function[nStates];

            m_debug = debug;
        }

        public void RegisterState(Enum State, Function OnEnter = null, Function OnUpdate = null, Function OnExit = null, Function OnFixedUpdate = null)
        {
            Int32 stateInt = Convert.ToInt32(State);
            m_onEnterFunc[stateInt] = OnEnter;
            m_onExitFunc[stateInt] = OnExit;
            m_onUpdateFunc[stateInt] = OnUpdate;
            m_onFixedUpdateFunc[stateInt] = OnFixedUpdate;
        }

        public void RegisterState(Enum state, IState stateInterface)
        {
            RegisterState(state, stateInterface.OnEnter, stateInterface.OnUpdate, stateInterface.OnExit, stateInterface.OnFixedUpdate);
        }

        public void SetState(Enum state)
        {
            int newState = Convert.ToInt32(state);
            m_NextState = newState;
            if (m_debug) Debug.Log("SET From " + m_currentState + " to " + m_NextState);
        }

        public bool IsState(Enum state)
        {
            return m_currentState == Convert.ToInt32(state);
        }
        public int GetState()
        {
            return m_currentState;
        }
        public int GetLastState()
        {
            return m_lastState;
        }
        public float GetStateTime()
        {
            return m_time;
        }
        public float GetStateTimeFixed()
        {
            return m_timeFixed;
        }

        public void Update()
        {
            m_time += Time.deltaTime;
            if (m_currentState != m_NextState)
            {
                if (m_currentState >= 0)
                {
                    Execute(m_onExitFunc[m_currentState]); //OnExit
                }
                if (m_debug) Debug.Log("From " + m_currentState + " to " + m_NextState);
                m_lastState = m_currentState;
                m_currentState = m_NextState;
                Execute(m_onEnterFunc[m_currentState]); //OnEnter
                m_time = 0;
                m_timeFixed = 0;
            }
            Execute(m_onUpdateFunc[m_currentState]); //OnUpdate
        }

        public void FixedUpdate()
        {
            if (m_currentState >= 0)
            {
                m_timeFixed += Time.fixedDeltaTime;
                Execute(m_onFixedUpdateFunc[m_currentState]); //OnFixedUpdate
            }
        }

        private void Execute(Function f)
        {
            if (f != null)
            {
                f();
            }
        }
    }
