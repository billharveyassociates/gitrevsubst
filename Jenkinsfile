pipeline {
  agent any
  stages {
    stage('build') {
      steps {
        bat 'echo %PATH%'
        bat 'echo env.MSBUILDEXE'
      }
    }
  }
  environment {
    MSBUILDEXE = 'C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\BuildTools\\MSBuild\\15.0\\Bin\\MSBuild.exe'
  }
}