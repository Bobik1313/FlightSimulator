#include "pch.h"

#ifdef ERROR
#undef ERROR
#endif

#include <FGFDMExec.h>
#include <simgear/misc/sg_path.hxx>
#include <string>

static JSBSim::FGFDMExec* g_fdm = nullptr;
static std::string g_lastError;

extern "C" __declspec(dllexport)
bool JSB_Init(
    const char* rootDir,
    const char* aircraftPath,
    const char* enginePath,
    const char* systemsPath)
{
    try
    {
        if (g_fdm != nullptr)
            return true;

        if (rootDir == nullptr ||
            aircraftPath == nullptr ||
            enginePath == nullptr ||
            systemsPath == nullptr)
        {
            g_lastError = "One or more JSBSim paths are null.";
            return false;
        }

        g_fdm = new JSBSim::FGFDMExec();

        g_fdm->SetRootDir(SGPath(rootDir));
        g_fdm->SetAircraftPath(SGPath(aircraftPath));
        g_fdm->SetEnginePath(SGPath(enginePath));
        g_fdm->SetSystemsPath(SGPath(systemsPath));

        return true;
    }
    catch (const std::exception& e)
    {
        g_lastError = e.what();
        return false;
    }
}

extern "C" __declspec(dllexport)
bool JSB_LoadAircraft(const char* aircraftName)
{
    try
    {
        if (g_fdm == nullptr)
        {
            g_lastError = "JSBSim is not initialized.";
            return false;
        }

        if (aircraftName == nullptr)
        {
            g_lastError = "Aircraft name is null.";
            return false;
        }

        return g_fdm->LoadModel(aircraftName);
    }
    catch (const std::exception& e)
    {
        g_lastError = e.what();
        return false;
    }
}

extern "C" __declspec(dllexport)
bool JSB_LoadScript(const char* scriptPath)
{
    try
    {
        if (g_fdm == nullptr)
        {
            g_lastError = "JSBSim is not initialized.";
            return false;
        }

        if (scriptPath == nullptr)
        {
            g_lastError = "Script path is null.";
            return false;
        }

        return g_fdm->LoadScript(SGPath(scriptPath));
    }
    catch (const std::exception& e)
    {
        g_lastError = e.what();
        return false;
    }
}

extern "C" __declspec(dllexport)
bool JSB_RunStep()
{
    try
    {
        if (g_fdm == nullptr)
        {
            g_lastError = "JSBSim is not initialized.";
            return false;
        }

        return g_fdm->Run();
    }
    catch (const std::exception& e)
    {
        g_lastError = e.what();
        return false;
    }
}

extern "C" __declspec(dllexport)
bool JSB_RunInitialConditions()
{
    try
    {
        if (g_fdm == nullptr)
        {
            g_lastError = "JSBSim is not initialized.";
            return false;
        }

        g_fdm->RunIC();
        return true;
    }
    catch (const std::exception& e)
    {
        g_lastError = e.what();
        return false;
    }
}

extern "C" __declspec(dllexport)
double JSB_GetProperty(const char* propertyName)
{
    try
    {
        if (g_fdm == nullptr || propertyName == nullptr)
            return 0.0;

        return g_fdm->GetPropertyValue(propertyName);
    }
    catch (...)
    {
        return 0.0;
    }
}

extern "C" __declspec(dllexport)
bool JSB_SetProperty(const char* propertyName, double value)
{
    try
    {
        if (g_fdm == nullptr)
        {
            g_lastError = "JSBSim is not initialized.";
            return false;
        }

        if (propertyName == nullptr)
        {
            g_lastError = "Property name is null.";
            return false;
        }

        g_fdm->SetPropertyValue(propertyName, value);
        return true;
    }
    catch (const std::exception& e)
    {
        g_lastError = e.what();
        return false;
    }
}

extern "C" __declspec(dllexport)
const char* JSB_GetLastError()
{
    return g_lastError.c_str();
}

extern "C" __declspec(dllexport)
void JSB_Shutdown()
{
    delete g_fdm;
    g_fdm = nullptr;
}