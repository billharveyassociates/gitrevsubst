pipeline {
  agent any
  triggers { pollSCM('/2 * * * ') }
  stages {
    stage('build') {
      steps {
        bat '"%MSBUILDEXE%" gitrevsubst.sln /p:Configuration=Release'
      }
    }
  }
  environment {
    MSBUILDEXE = 'C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\BuildTools\\MSBuild\\15.0\\Bin\\MSBuild.exe'
  }
}