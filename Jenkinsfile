pipeline {
  agent any
  stages {
    stage('build') {
      steps {
          script {
			bat '%MSBUILD15% gitrevsubst.sln /p:Configuration=Release'
			def template = readFile "${env.WORKSPACE}/changelog/incremental_template.txt"
			def incremental = gitChangelog from: [type: 'COMMIT', value: "${GIT_PREVIOUS_COMMIT}"], returnType: 'STRING', template: template, to: [type: 'COMMIT', value: "${GIT_COMMIT}"]
			currentBuild.description = incremental
		  }
      }
    }
  }
}